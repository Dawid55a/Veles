using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
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

        private ObservableCollection<MessageDto> messageList;
        private string userMessage = "";

        public ObservableCollection<MessageDto> MessageList
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
                .WithUrl("http://localhost:5152/chathub")
                .Build();

            string t = null;

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            OpenConnectionAsync();

        }

        private async void ExecuteSend(object obj)
        {
            //messageList.Add(new MessageDto("Def", userMessage));
            try
            {
                await connection.InvokeAsync("SendMessageTest",
                    Properties.Settings.Default.Username, UserMessage);

                UserMessage = "";
            }
            catch (Exception ex)
            {
                MessageDto errorMessage = new MessageDto();
                errorMessage.Text = ex.Message;
                errorMessage.CreatedDate = DateTime.Now;
                messageList.Add(errorMessage);
            }
            //message.User.
        }

        public async Task<ObservableCollection<MessageDto>> GetMessageListAsync()
        {
            ObservableCollection<MessageDto> messages = new ObservableCollection<MessageDto>();

            var result = RestApiMethods.GetCall("Messages/Group/"+group.Name);

            if(result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                messages = JsonConvert.DeserializeObject<ObservableCollection<MessageDto>>(jsonResult);
            }

            


            return messages;
        }

        private async void OpenConnectionAsync()
        {
            connection.On<string, string>("ReceiveMessageTest", (user, message) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    MessageDto messageDto = new MessageDto();
                    messageDto.Text = message;
                    //messageDto.User.UserName = user;
                    messageList.Add(messageDto);
                });
            });

            try
            {
                await connection.StartAsync();
                
            }
            catch (Exception ex)
            {
                MessageDto errorMessage = new MessageDto();
                errorMessage.Text = ex.Message;
                errorMessage.CreatedDate = DateTime.Now;
                messageList.Add(errorMessage);
            }
        }
        //public ChatViewModel
    }
}
