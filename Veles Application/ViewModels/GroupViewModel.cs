using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Veles_Application.Commands;
using Veles_Application.Models;
using Veles_Application.WepAPI;
using VelesLibrary.DTOs;

namespace Veles_Application.ViewModels
{
    public class GroupViewModel : BaseViewModel
    {
        private ObservableCollection<GroupDto> _groupList;

        public ObservableCollection<GroupDto> GroupList 
        {
            get { return _groupList; }
            set 
            { 
                _groupList = value; 
                OnPropertyChanged(nameof(GroupList));
            }
        }

        public ICommand ChangeGroupCommand { get; }
        public GroupViewModel()
        {
            GroupList = GetGroupsAsync().Result;
            ChangeGroupCommand = new ViewModelCommand(ExecuteGroupChange);
            EventsAggregator.OnMessageTransmitted += OnMessageRecived;
        }

        private void OnMessageRecived(object obj)
        {
            if(obj != null && obj is string)
            {
                if(obj.ToString() == "Delete")
                {
                    GroupList = GetGroupsAsync().Result;
                }
            }
        }

        //Trigger event in MainViewModel when group has changed
        private void ExecuteGroupChange(object obj)
        {
            EventsAggregator.SendMessage(obj);
        }
        
        //Get group list from api
        public async Task<ObservableCollection<GroupDto>> GetGroupsAsync()
        {
            ObservableCollection<GroupDto> groups = new ObservableCollection<GroupDto>(); ;

            var result = RestApiMethods.GetCallAuthorization("Groups/User/" + Properties.Settings.Default.Username);//connect to api

            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                groups = JsonConvert.DeserializeObject<ObservableCollection<GroupDto>>(jsonResult);
                return groups;
            }
            else
            {
                MessageBox.Show("connection interrupted");
                return groups;
            }
        }
    }
}
