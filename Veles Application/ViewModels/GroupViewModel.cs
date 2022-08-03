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
        public ObservableCollection<Group> GroupList { get; set; }

        public ICommand ChangeGroupCommand { get; }
        public GroupViewModel()
        {
            GroupList = GetGroupsAsync().Result;

            ChangeGroupCommand = new ViewModelCommand(ExecuteGroupChange);
        }

        private void ExecuteGroupChange(object obj)
        {
            EventsAggregator.BroadCast(obj);
        }

        private async Task<ObservableCollection<Group>> GetGroupsAsync()
        {
            ObservableCollection<Group> groups = new ObservableCollection<Group>(); ;

            var result = RestApiMethods.GetCallAuthorization("Groups/User/" + Properties.Settings.Default.Username);

            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                
                groups = JsonConvert.DeserializeObject<ObservableCollection<Group>>(jsonResult);
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
