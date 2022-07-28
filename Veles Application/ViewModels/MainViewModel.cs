using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Veles_Application.Commands;
using Veles_Application.Models;

namespace Veles_Application.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Group> GroupList { get; set; }

        //public BaseViewModel groupModel = new GroupViewModel();
        public BaseViewModel midViewModel = new HomeViewModel();//set mid panel

        public BaseViewModel MidViewModel
        {
            get { return midViewModel; }
            set { 
                midViewModel = value; 
                OnPropertyChanged(nameof(MidViewModel));
            }
        }

        public ICommand ListBoxSelectedCommand { get; }
        public ICommand ChangePanelCommand { get; }

        //Constructor
        public MainViewModel()
        {
            string test = Properties.Settings.Default.Username;
            GroupList = GetGroupsAsync().Result;

            //Add group to list
            //when you implement communication remove and add via server 
            //GroupList = recive(ServerGroupList as GroupList)
            //GroupList.Add(new Group("Jaga"));
            //GroupList.Add(new Group("Perun"));
            //GroupList.Add(new Group("Bies"));

            ListBoxSelectedCommand = new ViewModelCommand(ExecuteChangeGroup);
            ChangePanelCommand = new ViewModelCommand(ExecutePanelChange);
        }

        private void ExecutePanelChange(object parameter)
        {
            if (parameter.ToString() == "Home")
                MidViewModel = new HomeViewModel();
            else if(parameter != null)
            {
                MidViewModel = new ChatViewModel(parameter as Group);
            }
            //else if (parameter.ToString() == "Options") needs implementation

        }

        private void ExecuteChangeGroup(object parameter)
        {
            //System.Diagnostics.Debug.WriteLine(ListBoxItem.Content.ToString());
            
        }

        private async Task<ObservableCollection<Group>> GetGroupsAsync()
        {
            ObservableCollection<Group> groups = new ObservableCollection<Group>();
            groups.Add(new Group("Jaga"));
            groups.Add(new Group("Perun"));
            groups.Add(new Group("Bies"));
            return groups;
        }
    }
}
