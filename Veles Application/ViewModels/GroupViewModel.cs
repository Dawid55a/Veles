﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veles_Application.Models;

namespace Veles_Application.ViewModels
{
    public class GroupViewModel : BaseViewModel
    {
        public ObservableCollection<Group> GroupList { get; set; }
        public ObservableCollection<Message> MessageList { get; set; }

        public GroupViewModel()
        {
            GroupList = new ObservableCollection<Group>();
            GroupList.Add(new Group("Jaga"));
            GroupList.Add(new Group("Perun"));
            GroupList.Add(new Group("Bies"));

            MessageList = new ObservableCollection<Message>();
            MessageList.Add(new Message("Adam111", "Test"));
            MessageList.Add(new Message("PolskiHusarz2011", "polska GUROM"));
            MessageList.Add(new Message("Adam111", "?"));
        }
        
    }
}