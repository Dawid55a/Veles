using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Veles_Application.Commands;
using Veles_Application.Models;
using Veles_Application.WepAPI;
using VelesLibrary.DTOs;

namespace Veles_Application.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public Group group { get; set; }

        private ObservableCollection<NewMessageDto> messageList;
        private string userMessage = "";

        public ObservableCollection<NewMessageDto> MessageList
        {
            get { return messageList; }
            set 
            { 
                messageList = value; 
                OnPropertyChanged(nameof(MessageList));
            }
        }

        public string UserMessage
        {
            get { return userMessage; }
            set 
            { 
                userMessage = value;
                OnPropertyChanged(nameof(UserMessage));
            }
        }

        //RelayCommand
        public ICommand SendMessageCommand { get; }

        //Constructor
        HubConnection connection;
        public ChatViewModel(Group group)
        {
            this.group = group;
            MessageList = GetMessageListAsync().Result;

            SendMessageCommand = new ViewModelCommand(ExecuteSend);

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5152/chathub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(Properties.Settings.Default.Token);
                })
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            
            
            //OpenConnectionAsync();

        }

        
        private async void ExecuteSend(object obj)
        {
            //messageList.Add(new NewMessageDto("Def", userMessage));
            try
            {
                var createMessageDto = new CreateMessageDto()
                {
                    Content = UserMessage,
                    Sender = Properties.Settings.Default.Username,
                    Created = DateTime.UtcNow,
                    GroupName = group.Name
                };
                await connection.InvokeAsync("SendMessage", createMessageDto);

                UserMessage = "";
            }
            catch (Exception ex)
            {
                NewMessageDto errorNewMessage = new NewMessageDto();
                errorNewMessage.Text = ex.Message;
                errorNewMessage.CreatedDate = DateTime.Now;
                messageList.Add(errorNewMessage);
            }
            //message.User.
        }
        

        public async Task<ObservableCollection<NewMessageDto>> GetMessageListAsync()
        {
            ObservableCollection<NewMessageDto> messages = new ObservableCollection<NewMessageDto>();

            var result = RestApiMethods.GetCallAuthorization("Messages/Group/"+group.Name);

            if(result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                messages = JsonConvert.DeserializeObject<ObservableCollection<NewMessageDto>>(jsonResult);
            }

            return messages;
        }

        //Receive message from server
        public async void OpenConnectionAsync()
        {
            connection.On<NewMessageDto>("NewMessage", (newMessageDto) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (newMessageDto.Group == group.Name)
                    {
                        messageList.Add(newMessageDto);
                    }
                    
                });
            });

            try
            {
                await connection.StartAsync();
                
            }
            catch (Exception ex)
            {
                NewMessageDto newMessageDto = new NewMessageDto();
                newMessageDto.Text = ex.Message;
                newMessageDto.CreatedDate = DateTime.Now;
                messageList.Add(newMessageDto);
            }
        }
        //public ChatViewModel
    }
}
