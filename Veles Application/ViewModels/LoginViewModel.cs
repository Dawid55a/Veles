using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Veles_Application.Commands;
using Veles_Application.Models;
using VelesLibrary.DTOs;
using Veles_Application.WepAPI;

namespace Veles_Application.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username = "Karol";
        private string _password = "1234";
        private string _errorMessage;
        private bool _isViewVisible = true;

        public string Username
        {
            get { return _username; }
            set 
            { 
                _username = value; 
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return _password; }
            set { 
                _password = value; 
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { 
                _errorMessage = value; 
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { 
                _isViewVisible = value; 
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        public ICommand LoginCommand { get; } 

        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);//Check Commands/ViewModelCommand
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            //add validation if need
            return true;
        }

        private async void ExecuteLoginCommand(object obj)
        {
            LoginDto loginDto = new LoginDto();
            loginDto.UserName = Username;
            loginDto.Password = Password;

            //send login request to API
            var result = await Task.Run(()=> RestApiMethods.PostCall("Account/Login", loginDto));
            
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read json
                string jsonResult = result.Content.ReadAsStringAsync().Result;
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(jsonResult);

                //checking if the API returned correct data
                if (userDto == null)
                {
                    MessageBox.Show("Server return empty object", "Login error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    //Save to setting
                    Properties.Settings.Default.Username = userDto.UserName;
                    Properties.Settings.Default.Token = userDto.Token;
                }
 
                //Close window
                IsViewVisible = false;
            }
            else
            {
                MessageBox.Show("Incorrect username or password\n Status code: " + result.StatusCode.ToString(), "Login error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
           //changes visibility to close LoginView

        }
    }
}
