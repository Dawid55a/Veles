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
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace Veles_Application.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        public ObservableCollection<string> UsersList { get; set; }
        public bool IsOwner = false; 
        private GroupDto group;

        public ICommand DeleteGroupCommand { get; }
        public UsersViewModel(GroupDto group)
        {
            this.group = group;
            if (group != null && group.Owner == Properties.Settings.Default.Username)
                IsOwner = true;

            UsersList = GetUsersAsync().Result;

            DeleteGroupCommand = new ViewModelCommand(ExecuteDeleteGroup, CanEcecuteDeleteGroup);
        }

        private void ExecuteDeleteGroup(object obj)
        {
            var result = RestApiMethods.DeleteCallAuthorization("Groups/" + group.Id);

            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("The group has been deleted", "Succes", 
                    MessageBoxButton.OK, MessageBoxImage.Information);

                EventsAggregator.SendMessage("Delete");
            }
            else
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                Methods.Messages.BadRequest(jsonResult);
            }

        }

        private bool CanEcecuteDeleteGroup(object obj)
        {
            if(group != null && group.Owner == Properties.Settings.Default.Username)
                return true;
            else
                return false;
        }

        public async Task<ObservableCollection<string>> GetUsersAsync()
        {
            ObservableCollection<string> users = new ObservableCollection<string>(); ;

            var result = RestApiMethods.GetCallAuthorization("Users/Group/" + group.Name);

            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;

                users = JsonConvert.DeserializeObject<ObservableCollection<string>>(jsonResult);
                return users;
            }
            else
            {
                MessageBox.Show("connection interrupted");
                return users;
            }
        }
    }
}
