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
using System.Drawing;

namespace Veles_Application.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        //Change password
        private string _oldPassword = "";
        private bool _isOldPasswordWatermarkVisible = true;

        private string _newPassword = "";
        private bool _isNewPasswordWatermarkVisible = true;

        private string _errorMessage = "";

        //Change nick
        private string _newNick = "";
        private Group? _selectedGroup = null;


        public string OldPassword
        { 
            get { return _oldPassword; } 
            set 
            { 
                _oldPassword = value;
                IsOldPasswordWatermarkVisible = string.IsNullOrEmpty(_oldPassword);
                OnPropertyChanged(nameof(OldPassword));
            }
        }

        public bool IsOldPasswordWatermarkVisible
        {
            get { return _isOldPasswordWatermarkVisible; }
            set 
            { 
                if (value.Equals(_isOldPasswordWatermarkVisible)) return;
                _isOldPasswordWatermarkVisible = value;
                OnPropertyChanged(nameof(IsOldPasswordWatermarkVisible));
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                IsNewPasswordWatermarkVisible = string.IsNullOrEmpty(_newPassword);
                OnPropertyChanged(nameof(OldPassword));
            }
        }

        public bool IsNewPasswordWatermarkVisible
        {
            get { return _isNewPasswordWatermarkVisible; }
            set 
            {
                if (value.Equals(_isNewPasswordWatermarkVisible)) return;
                _isNewPasswordWatermarkVisible = value;
                OnPropertyChanged(nameof(IsNewPasswordWatermarkVisible));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                
                OnPropertyChanged(nameof(_errorMessage));
            }
        }

        public string NewNick
        {
            get { return _newNick; }
            set
            {
                _newNick = value;
                OnPropertyChanged(nameof(NewNick));
            }
        }

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }

        public ObservableCollection<Group> GroupList { get; set; }

        public ICommand ChangePasswordCommand { get; }
        public ICommand ChangeNickCommand { get; }

        public SettingsViewModel()
        {
            GroupViewModel group = new GroupViewModel();
            GroupList = group.GroupList;
            

            ChangePasswordCommand = new ViewModelCommand(ExecuteChangePassword);
            ChangeNickCommand = new ViewModelCommand(ExecuteChangeNick);

        }

        private async void ExecuteChangePassword(object parameter)
        {
            if (OldPassword == NewPassword)
            {
                MessageBox.Show("the new password cannot be the same as the old one", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
               
            }
            else if (OldPassword == "" || NewPassword == "")
            {
                MessageBox.Show("password cannot be empty", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            else
            {
                //connection
            }
        }

        private async void ExecuteChangeNick(object parameter)
        {
            if(NewNick == "")
            {
                MessageBox.Show("nick cannot be empty", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(SelectedGroup == null)
            {
                MessageBox.Show("please select a group", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //connection
            }
        }
    }
}
