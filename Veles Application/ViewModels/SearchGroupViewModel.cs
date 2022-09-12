using Newtonsoft.Json;
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
using Veles_Application.WepAPI;
using VelesLibrary.DTOs;

namespace Veles_Application.ViewModels
{
    public class SearchGroupViewModel : BaseViewModel
    {
        private string _searchName = "";
        
        public string SearchName
        {
            get { return _searchName; }
            set
            {
                _searchName = value;
                OnPropertyChanged(nameof(SearchName));
            }
        }

        public ObservableCollection<GroupDto> GroupList { get; set; }
        public ObservableCollection<GroupDto> OwnGroupList { get; set; }

        public ICommand SearchGroupCommand { get; }
        public ICommand JoinToGroupCommand { get; }
        public SearchGroupViewModel()
        {
            SearchGroupCommand = new ViewModelCommand(ExecuteSearchGroup);
            JoinToGroupCommand = new ViewModelCommand(ExecuteJoinToGroup);

            GroupList = new ObservableCollection<GroupDto>();
            GroupViewModel group = new GroupViewModel();
            OwnGroupList = group.GetGroupsAsync().Result;
        }

        private async void ExecuteSearchGroup(object parameter)
        {   
            ObservableCollection<GroupDto> groups = GetGroupLikeAsync().Result;
            GroupList.Clear();

            var groupsWithoutOwned = groups.Where(g => !OwnGroupList.Any(o => g.Name == o.Name));

            foreach (GroupDto group in groupsWithoutOwned)
            {
                GroupList.Add(group);
            }
        }

        private async void ExecuteJoinToGroup(object parameter)
        {
            try
            {
                AddToGroupDto addToGroup = new AddToGroupDto(){
                    GroupName = parameter.ToString(),
                    UserName = Properties.Settings.Default.Username
                };
                var result = RestApiMethods.PostCallAuthorization("Account/add_to_group", addToGroup);

                if(result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("You have been added to the group", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string jsonResult = result.Result.Content.ReadAsStringAsync().Result;
                    ResponseDto responseDto = JsonConvert.DeserializeObject<ResponseDto>(jsonResult);
                    if(responseDto == null)
                        responseDto = new ResponseDto() { Message = result.Result.ReasonPhrase};
                    MessageBox.Show(responseDto.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something goes wrong", "Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            

            
        }

        //Search group with similar names
        public async Task<ObservableCollection<GroupDto>> GetGroupLikeAsync()
        {
            ObservableCollection<GroupDto> groups = new ObservableCollection<GroupDto>(); ;

            var result = RestApiMethods.GetCallAuthorization("Groups/Name/" + SearchName);

            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonResult = result.Result.Content.ReadAsStringAsync().Result;

                groups = JsonConvert.DeserializeObject<ObservableCollection<GroupDto>>(jsonResult);
                return groups;
            }
            else if(result.Result.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("Not found", "Not found", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return groups;
            }
            
            return groups;
        }
    }
}
