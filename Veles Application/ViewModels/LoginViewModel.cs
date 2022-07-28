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

        public static Task<HttpResponseMessage> GetCall(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //string apiUrl = API_URIs.baseURI + url;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(url);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Task<HttpResponseMessage> PostCall<T>(string url, T model) where T : class
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //string apiUrl = API_URIs.baseURI + url;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response =  client.PostAsJsonAsync(url, model);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            //add validation 
            return true;
        }

        private async void ExecuteLoginCommand(object obj)
        {
            //add authenticate

            //Thread.CurrentPrincipal = new GenericPrincipal(
                //new GenericIdentity(Username), null);//przechwouje username w generycznej wątku nie wiem jak za bardzo to dziala, choc sie domyslam

            LoginDto loginDto = new LoginDto();
            loginDto.UserName = Username;
            loginDto.Password = Password;
            var result = await Task.Run(()=> PostCall("http://localhost:5152/api/Account/Login", loginDto));
            
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read json
                string jsonResult = result.Content.ReadAsStringAsync().Result;
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(jsonResult);

                if(userDto == null)
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

                string test = Properties.Settings.Default.Token;
                System.Diagnostics.Debug.WriteLine("OK"); 
                //Properties.Settings.Default.Username = 
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
