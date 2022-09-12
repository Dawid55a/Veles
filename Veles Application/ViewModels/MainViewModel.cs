using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using VelesLibrary.DTOs;

namespace Veles_Application.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string userName = Properties.Settings.Default.Username;

        private bool _isViewVisible = true;

        private bool isSearchClicked = false;
        private bool isCreateClicked = false;

        private BaseViewModel midViewModel = new HomeViewModel();//set mid panel
        private BaseViewModel leftViewModel = new GroupViewModel();
        private BaseViewModel? rightViewModel;

        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

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

        public BaseViewModel RightViewModel
        {
            get { return rightViewModel; }
            set
            {
                rightViewModel = value;
                OnPropertyChanged(nameof (RightViewModel));
            }
        }

        public ICommand ListBoxSelectedCommand { get; }
        public ICommand ChangePanelCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand ChangeLeftPanelCommand { get; }
        public object DispacherOperation { get; private set; }

        //Constructor
        public MainViewModel()
        {
            ListBoxSelectedCommand = new ViewModelCommand(ExecuteChangeGroup);
            ChangePanelCommand = new ViewModelCommand(ExecutePanelChange);
            ChangeLeftPanelCommand = new ViewModelCommand(ExecuteLeftPanelChange);
            OpenSettingsCommand = new ViewModelCommand(ExecuteOpenSettings);

            Task.Factory.StartNew(() => 
            {
                FirstGroupOrDefaultAsync();
            });

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
            else if (parameter.ToString() == "Logout")
            {
                //MidViewModel = new HomeViewModel();
                //IsViewVisible = false;
                EventsAggregator.SwitchPage(new LoginViewModel());

            }  
            else if (parameter.ToString() == "Settings")
            {
                MidViewModel = new SettingsViewModel();
                RightViewModel = null;
            }   
            else if (parameter != null)
            {
                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    ChatViewModel chatView = new ChatViewModel(parameter as GroupDto);
                    chatView.OpenConnectionAsync();
                    MidViewModel = chatView;

                });
                
                RightViewModel = new UsersViewModel(parameter as GroupDto);
                
            }

        }

        private async void ExecuteLeftPanelChange(object parameter)
        {
            if(parameter.ToString() == "Search")
            {
                if(!isSearchClicked)
                {
                    LeftViewModel = new SearchGroupViewModel();
                    isSearchClicked = true;
                    isCreateClicked = false;
                }
                else
                {
                    LeftViewModel = new GroupViewModel();
                    isSearchClicked = false;
                }
            }
            else if(parameter.ToString() == "Create")
            {
                if (!isCreateClicked)
                {
                    LeftViewModel = new CreateGroupViewModel();
                    isSearchClicked = false;
                    isCreateClicked = true;
                }
                else
                {
                    LeftViewModel = new GroupViewModel();
                    isCreateClicked = false;
                }
            }
        }

        //Handle event from GroupViewModel
        private void OnMessageRecived(object obj)
        {   
            if(obj is GroupDto)
                ExecutePanelChange(obj);
            else
            {
                FirstGroupOrDefaultAsync();
            }
                
        }

        private void ExecuteChangeGroup(object parameter)
        {
            //System.Diagnostics.Debug.WriteLine(ListBoxItem.Content.ToString());

        }

        private async Task<ObservableCollection<GroupDto>> GetGroupsAsync()
        {
            ObservableCollection<GroupDto> groups;

            var result = RestApiMethods.GetCallAuthorization("Groups/User/"+userName);

            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                groups = JsonConvert.DeserializeObject<ObservableCollection<GroupDto>>(jsonResult);
                return groups;
            }
            else if(IsViewVisible)
            {
                MessageBox.Show("connection interrupted");
                
            }
            return groups = new ObservableCollection<GroupDto>();
        }

        //When find group switch mid view to ChatView, otherwise set view as HomeView
        private async void FirstGroupOrDefaultAsync()
        {
            ObservableCollection<GroupDto> groups;
            await Task.Run(() =>
            {
                 groups = GetGroupsAsync().Result;
                 var item = groups.FirstOrDefault();
                if (item != null)
                    ExecutePanelChange((GroupDto)item);
                else
                    MidViewModel = new HomeViewModel();

            });

            LeftViewModel = new GroupViewModel();

        }
    }
}
