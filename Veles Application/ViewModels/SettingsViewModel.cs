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
using Veles_Application.WepAPI;
using VelesLibrary.DTOs;
using Newtonsoft.Json;

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

        public ICommand DeleteUserCommand { get; }

        public SettingsViewModel()
        {
            GroupViewModel group = new GroupViewModel();
            GroupList = group.GroupList;
            

            ChangePasswordCommand = new ViewModelCommand(ExecuteChangePassword);
            ChangeNickCommand = new ViewModelCommand(ExecuteChangeNick);
            DeleteUserCommand = new ViewModelCommand(ExecuteDeleteUser);

        }

        private async void ExecuteChangePassword(object parameter)
        {
            var Result = MessageBox.Show("Are you sure you want to delete your account?", "Are you sure?",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
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
                    ChangePasswordDto changePasswordDto = new ChangePasswordDto()
                    {
                        OldPassword = OldPassword,
                        NewPassword = NewPassword,
                        UserName = Properties.Settings.Default.Username
                    };

                    var result = RestApiMethods.PostCallAutorization("Account/change_password", changePasswordDto);

                    //successfully changed the password
                    if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                        TokenDto token = JsonConvert.DeserializeObject<TokenDto>(jsonResult);

                        if (token != null)
                        {
                            Properties.Settings.Default.Username = token.UserName;
                            Properties.Settings.Default.Token = token.Token;

                            MessageBox.Show("successfully changed the password", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                        ResponseDto response = JsonConvert.DeserializeObject<ResponseDto>(jsonResult);

                        try
                        {
                            MessageBox.Show(response.Message, "Success",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch(ArgumentNullException)
                        {
                            MessageBox.Show("Somethig goes wrong", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            
        }

        private async void ExecuteChangeNick(object parameter)
        {
            var Result = MessageBox.Show("Are you sure you want to change your nick?", "Are you sure?",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
            {
                if (NewNick == "")
                {
                    MessageBox.Show("nick cannot be empty", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (SelectedGroup == null)
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

        private async void ExecuteDeleteUser(object parameter)
        {
            var Result = MessageBox.Show("Are you sure you want to delete your account?", "Are you sure?", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
            {
                //communication
            }
            
        }
    }
}
