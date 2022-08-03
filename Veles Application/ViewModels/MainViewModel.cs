using Newtonsoft.Json;
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

        //Constructor
        public MainViewModel()
        {
            ListBoxSelectedCommand = new ViewModelCommand(ExecuteChangeGroup);
            ChangePanelCommand = new ViewModelCommand(ExecutePanelChange);

            EventsAggregator.OnMessageTransmitted += OnMessageRecived;
        }

        private async void ExecutePanelChange(object parameter)
        {
            if (parameter == null)
                return;
            else if (parameter.ToString() == "Home")
                MidViewModel = new HomeViewModel();
            else if (parameter != null)
            {
                ChatViewModel chatView = new ChatViewModel(parameter as Group);
                chatView.OpenConnectionAsync();
                MidViewModel = chatView;
                //MidViewModel = new ChatViewModel(parameter as Group);
                
                
            }
            //else if (parameter.ToString() == "Options") needs implementation

        }
        private void OnMessageRecived(object obj)
        {   
            ExecutePanelChange(obj);
        }

        private void ExecuteChangeGroup(object parameter)
        {
            //System.Diagnostics.Debug.WriteLine(ListBoxItem.Content.ToString());

        }
    }
}
