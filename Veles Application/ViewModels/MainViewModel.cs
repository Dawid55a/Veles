using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Veles_Application.Commands;

namespace Veles_Application.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _Text;
        private string _username;// Thread.CurrentPrincipal.Identity.Name;
        private string _message;

        public string Text
        {
            get { return _Text; }
            set 
            { 
                _Text = value; 
                OnPropertyChanged(nameof(Text));
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Message
        {
            get { return _message; }
            set 
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public ICommand SendCommand { get; }

        public MainViewModel()
        {
            SendCommand = new ViewModelCommand(ExecuteLoginCommand);
        }

        //write to text
        private void ExecuteLoginCommand(object obj)
        {
            Text += Username + ": " + Message + "\n" ;
            Message = "";
        }
    }
}
