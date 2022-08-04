﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Veles_Application.Commands;
using Veles_Application.Models;
using Veles_Application.WepAPI;

namespace Veles_Application.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string userName = Properties.Settings.Default.Username;

        //public BaseViewModel groupModel = new GroupViewModel();
        public BaseViewModel midViewModel = new HomeViewModel();//set mid panel
        public BaseViewModel leftViewModel = new GroupViewModel();

        public BaseViewModel MidViewModel
        {
            get { return midViewModel; }
            set
            {
                midViewModel = value;
                OnPropertyChanged(nameof(MidViewModel));
            }
        }

        public BaseViewModel LeftViewModel
        {
            get { return leftViewModel; }
            set
            {
                leftViewModel = value;
                OnPropertyChanged(nameof(leftViewModel));
            }
        }

        public ICommand ListBoxSelectedCommand { get; }
        public ICommand ChangePanelCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        //Constructor
        public MainViewModel()
        {
            ListBoxSelectedCommand = new ViewModelCommand(ExecuteChangeGroup);
            ChangePanelCommand = new ViewModelCommand(ExecutePanelChange);
            OpenSettingsCommand = new ViewModelCommand(ExecuteOpenSettings);

            EventsAggregator.OnMessageTransmitted += OnMessageRecived;
        }

        private void ExecuteOpenSettings(object parameter)
        {
            SettingsViewModel settings = new SettingsViewModel();
            var win = new Window()
            {
                DataContext = settings,
                Height = 300,
                Width = 300,
                ResizeMode = ResizeMode.NoResize,
            };
            win.Show();

        }

        private async void ExecutePanelChange(object parameter)
        {
            if (parameter == null)
                return;
            else if (parameter.ToString() == "Home")
                MidViewModel = new HomeViewModel();
            else if (parameter.ToString() == "Settings")
                MidViewModel = new SettingsViewModel();
            else if (parameter != null)
            {
                ChatViewModel chatView = new ChatViewModel(parameter as Group);
                chatView.OpenConnectionAsync();
                MidViewModel = chatView;
                //MidViewModel = new ChatViewModel(parameter as Group)
            }
            //else if (parameter.ToString() == "Options") needs implementation

        }

        //Handle event from GroupViewModel
        private void OnMessageRecived(object obj)
        {   
            if(obj is Group)
                ExecutePanelChange(obj);
        }

        private void ExecuteChangeGroup(object parameter)
        {
            //System.Diagnostics.Debug.WriteLine(ListBoxItem.Content.ToString());

        }

        private async Task<ObservableCollection<Group>> GetGroupsAsync()
        {
            ObservableCollection<Group> groups;

            var result = RestApiMethods.GetCall("UserGroups/User/"+userName);

            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                groups = JsonConvert.DeserializeObject<ObservableCollection<Group>>(jsonResult);
                return groups;
            }
            else
            {
                MessageBox.Show("connection interrupted");
                return groups = new ObservableCollection<Group>();
            }
        }
    }
}
