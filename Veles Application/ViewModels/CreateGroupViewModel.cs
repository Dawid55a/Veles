using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Veles_Application.Commands;
using Veles_Application.WepAPI;
using VelesLibrary.DTOs;

namespace Veles_Application.ViewModels
{
    public class CreateGroupViewModel : BaseViewModel
    {
        private string _createGroupName;

        public string CreateGroupName
        {
            get { return _createGroupName; }
            set
            {
                _createGroupName = value;
                OnPropertyChanged(nameof(CreateGroupName));
            }
        }

        public ICommand CreateGroupCommand { get; }

        public CreateGroupViewModel()
        {
            CreateGroupCommand = new ViewModelCommand(ExecuteCreateGroup);
        }

        private void ExecuteCreateGroup(object parameter)
        {
            CreateGroupDto createGroup = new CreateGroupDto()
            {
                Name = CreateGroupName
            };

            var result = RestApiMethods.PostCallAuthorization("Groups", createGroup);//connect to api

            if(result.Result.StatusCode == System.Net.HttpStatusCode.OK || result.Result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                MessageBox.Show("The group " + CreateGroupName + " was created", "Succes",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                Methods.Messages.BadRequest(jsonResult);
            }

        }
    }
}
