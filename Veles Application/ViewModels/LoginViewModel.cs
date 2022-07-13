using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Veles_Application.Commands;

namespace Veles_Application.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
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
            //add validation 
            return true;
        }

        private void ExecuteLoginCommand(object obj)
        {
            //add authenticate
            Thread.CurrentPrincipal = new GenericPrincipal(
                new GenericIdentity(Username), null);//przechwouje username w generycznej wątku nie wiem jak za bardzo to dziala, choc sie domyslam
            IsViewVisible = false;//changes visibility to close LoginView
        }
    }
}
