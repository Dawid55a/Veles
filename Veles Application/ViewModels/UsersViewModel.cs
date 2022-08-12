using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Veles_Application.Models;
using Veles_Application.WepAPI;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace Veles_Application.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        public ObservableCollection<string> UsersList { get; set; }

        public UsersViewModel(Models.Group group)
        {
            UsersList = GetUsersAsync(group).Result;
        }


        public async Task<ObservableCollection<string>> GetUsersAsync(Models.Group group)
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
