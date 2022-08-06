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

namespace Veles_Application.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        public ObservableCollection<User> UsersList { get; set; }

        public UsersViewModel(Models.Group group)
        {
            UsersList = GetUsersAsync(group).Result;
        }


        public async Task<ObservableCollection<User>> GetUsersAsync(Models.Group group)
        {
            ObservableCollection<User> users = new ObservableCollection<User>(); ;

            var result = RestApiMethods.GetCallAuthorization("Users/Group/" + group.Name);

            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;

                users = JsonConvert.DeserializeObject<ObservableCollection<User>>(jsonResult);
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
