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
        //For login
        private string _username = "Karol";
        private string _password = "1234";
        private string _errorMessage;

        //For registration
        private string _registrationUsername;
        private string _registrationPassword;
        private string _registrationEmail;

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

        public string RegistrationUsername
        {
            get { return _registrationUsername; }
            set{
                _registrationUsername = value;
                OnPropertyChanged(nameof(RegistrationUsername));
            }
        }

        public string RegistrationPassword
        {
            get { return _registrationPassword; }
            set{
                _registrationPassword = value;
                OnPropertyChanged(nameof(RegistrationPassword));
            }
        }

        public string RegistrationEmail
        {
            get { return _registrationEmail; }
            set { 
                _registrationEmail = value;
                OnPropertyChanged(nameof(RegistrationEmail));
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
        public ICommand RegistrationCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);//Check Commands/ViewModelCommand
            RegistrationCommand = new ViewModelCommand(ExecuteRegistrationCommand, CanExecuteRegistrationCommand);
        }

        

        private bool CanExecuteLoginCommand(object obj)
        {
            //add validation if need
            return true;
        }

        //Login command
        private async void ExecuteLoginCommand(object obj)
        {
            LoginDto loginDto = new LoginDto();
            loginDto.UserName = Username;
            loginDto.Password = Password;

            //send login request to API
            var result = await Task.Run(()=> RestApiMethods.PostCall("Account/Login", loginDto));
            
            if (result.StatusCode == HttpStatusCode.OK)
            {
                //Read json
                string jsonResult = result.Content.ReadAsStringAsync().Result;
                TokenDto tokenDto = JsonConvert.DeserializeObject<TokenDto>(jsonResult);

                //checking if the API returned correct data
                if (tokenDto == null)
                {
                    MessageBox.Show("Server return empty object", "Login error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    //Save to setting
                    Properties.Settings.Default.Username = tokenDto.UserName;
                    Properties.Settings.Default.Token = tokenDto.Token;
                }
 
                //Close window
                IsViewVisible = false;
                
            }
            else
            {
                MessageBox.Show("Incorrect username or password\n Status code: " + result.StatusCode.ToString(), "Login error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Registration Command
        private bool CanExecuteRegistrationCommand(object obj)
        {
            //add validation
            return true;
        }

        private async void ExecuteRegistrationCommand(object obj)
        {
            RegisterDto registerDto = new RegisterDto();
            registerDto.UserName = RegistrationUsername;
            registerDto.Password = RegistrationPassword;
            registerDto.Email = RegistrationEmail;

            var result = await Task.Run(() => RestApiMethods.PostCall("Account/Register", registerDto));
            if(result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created)
            {
                //Read json
                string jsonResult = result.Content.ReadAsStringAsync().Result;
                TokenDto tokenDto = JsonConvert.DeserializeObject<TokenDto>(jsonResult);

                //checking if the API returned correct data
                if (tokenDto == null)
                {
                    MessageBox.Show("Server return empty object", "Login error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    //Save to setting
                    Properties.Settings.Default.Username = tokenDto.UserName;
                    Properties.Settings.Default.Token = tokenDto.Token;
                }

                //Close window
                IsViewVisible = false;
            }
            else
            {
                MessageBox.Show("Incorrect username or password\n", "Registration error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
