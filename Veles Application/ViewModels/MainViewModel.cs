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
        public ObservableCollection<Group> GroupList { get; set; }

        //public BaseViewModel groupModel = new GroupViewModel();
        public BaseViewModel midViewModel = new HomeViewModel();//set mid panel

        public BaseViewModel MidViewModel
        {
            get { return midViewModel; }
            set
            {
                midViewModel = value;
                OnPropertyChanged(nameof(MidViewModel));
            }
        }

        public ICommand ListBoxSelectedCommand { get; }
        public ICommand ChangePanelCommand { get; }

        //Constructor
        public MainViewModel()
        {
            GroupList = GetGroupsAsync().Result;

            ListBoxSelectedCommand = new ViewModelCommand(ExecuteChangeGroup);
            ChangePanelCommand = new ViewModelCommand(ExecutePanelChange);
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

        private void ExecuteChangeGroup(object parameter)
        {
            //System.Diagnostics.Debug.WriteLine(ListBoxItem.Content.ToString());

        }

        private async Task<ObservableCollection<Group>> GetGroupsAsync()
        {
            ObservableCollection<Group> groups;

            var result = RestApiMethods.GetCall("Groups");

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
