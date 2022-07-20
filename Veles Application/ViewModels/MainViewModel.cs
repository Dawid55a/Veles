using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        private ListBoxItem listBoxItem;

        public ListBoxItem ListBoxItem
        {
            get { return listBoxItem; }
            set { listBoxItem = value; OnPropertyChanged(nameof(ListBoxItem)); }
        }

        //public BaseViewModel groupModel = new GroupViewModel();
        public BaseViewModel midViewModel = new HomeViewModel();

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

        public MainViewModel()
        {
            GroupList = new ObservableCollection<Group>();
            GroupList.Add(new Group("Jaga"));
            GroupList.Add(new Group("Perun"));
            GroupList.Add(new Group("Bies"));

            ListBoxSelectedCommand = new ViewModelCommand(ExecuteChangeGroup);
            ChangePanelCommand = new ViewModelCommand(ExecutePanelChange);
        }

        private void ExecutePanelChange(object parameter)
        {
            if (parameter.ToString() == "Home")
                MidViewModel = new HomeViewModel();
            //else if (parameter.ToString() == "Options") needs implementation

        }

        private void ExecuteChangeGroup(object parameter)
        {
            //System.Diagnostics.Debug.WriteLine(ListBoxItem.Content.ToString());
            MidViewModel = new ChatViewModel();
        }
    }
}
