using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veles_Application.Models;

namespace Veles_Application.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        public ObservableCollection<Users> UsersList { get; set; }

        public UsersViewModel()
        {
            UsersList = new ObservableCollection<Users>();
            UsersList.Add(new Users() { Name = "Karol"});
            UsersList.Add(new Users() { Name = "Adam" });
        }

        

    }
}
