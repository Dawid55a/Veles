using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Veles_Application.Commands;

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
            //communication
            MessageBox.Show("the group " + CreateGroupName + " was created");
        }
    }
}
