using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        public ICommand SendMessageCommand { get; }
        public ChatViewModel(Group group)
        {
            this.group = group;
            MessageList = GetMessageListAsync().Result;

            SendMessageCommand = new ViewModelCommand(ExecuteSend);

        }

        private void ExecuteSend(object obj)
        {
            //messageList.Add(new MessageDto("Def", userMessage));
            MessageDto message = new MessageDto();
            message.Text = userMessage;
            message.CreatedDate = DateTime.Now;
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
                //return messages;
            }
            //Recive message from group
            //messages.Add(new Message("Adam" + group.Name, "Hello in " + group.Name));
            //messages.Add(new Message("userfrom_"+group.Name, "Hello in" + group.Name));


            return messages;
        }
        //public ChatViewModel
    }
}
