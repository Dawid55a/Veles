﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Veles_Application.Commands;
using Veles_Application.Models;

namespace Veles_Application.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public Group group { get; set; }

        private ObservableCollection<Message> messageList;
        private string userMessage = "";

        public ObservableCollection<Message> MessageList
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
            messageList.Add(new Message("Def", userMessage));
            UserMessage = "";
        }

        public async Task<ObservableCollection<Message>> GetMessageListAsync()
        {
            ObservableCollection<Message> messages = new ObservableCollection<Message>();

            //Recive message from group
            //messages.Add(new Message("Adam" + group.Name, "Hello in " + group.Name));
            messages.Add(new Message("userfrom_"+group.Name, "Hello in" + group.Name));
            

            return messages;
        }
        //public ChatViewModel
    }
}