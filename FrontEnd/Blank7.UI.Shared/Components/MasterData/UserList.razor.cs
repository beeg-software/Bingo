// Namespace imports
using Blank7.Common.DomainModel.MasterData;
using Blank7.UI.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

// Define the UserList component within the Blank7.UI.Shared.Components.MasterData namespace
namespace Blank7.UI.Shared.Components.MasterData
{
    public partial class UserList
    {
        // Dependency injection
        [Inject] public IUserService UserService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        // Component properties
        protected List<User> Users = new List<User>();
        protected List<UserListUI> _userListUI = new List<UserListUI>();
        protected int _progCount = 1;
        protected bool _selectAll;
        protected bool _isLoading = true;


        // Lifecycle method to initialize the component
        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            // Fetch users and add them to the Users list
            Users = await UserService.GetUsersAsync();

            // Convert each user into a UserListUI instance and add it to the _userListUI list
            foreach (var us in Users)
            {
                _userListUI.Add(new UserListUI
                {
                    Id = us.Id,
                    IdString = us.Id.ToString(),
                    Name = us.Name
                });
            }

            _isLoading = false;
        }

        // Export data to CSV file
        protected async Task ExportToCSV()
        {
            var stringBuilder = new StringBuilder();

            // Write the header row
            stringBuilder.AppendLine("\"Prog\",\"Nome\"");

            // Write the data rows
            int progCount = 1;
            foreach (var user in _userListUI.OrderBy(us => us.Id))
            {
                stringBuilder.AppendLine($"\"{progCount}\",\"{user.Name}\"");
                progCount++;
            }

            // Save the CSV data as a string
            var csvData = stringBuilder.ToString();

            // Call the JavaScript method to download the CSV file
            await JSRuntime.InvokeVoidAsync("downloadCSV", "UserList.csv", csvData);
        }

        // Method to toggle the selection of all users in the list
        protected void ToggleSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var user in _userListUI)
            {
                user.Selected = _selectAll;
            }
        }

        // Method to navigate to the user creation page
        protected void NavigateToNewUser()
        {
            NavigationManager.NavigateTo($"/Masterdata/UserDetails/{Guid.Empty}");
        }

        // Method to confirm the deletion of selected users
        protected async Task ConfirmDeleteUsers()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the selected users?");
            if (confirmed)
            {
                await DeleteSelectedUsers();
            }
        }

        // Method to delete selected users
        protected async Task DeleteSelectedUsers()
        {
            // Filter the selected users
            var selectedUsers = _userListUI.Where(u => u.Selected).ToList();

            // Create a copy of the _userListUI collection
            var userListUICopy = _userListUI.ToList();

            // Iterate through the selected users and delete them
            foreach (var user in selectedUsers)
            {
                bool isDeleted = await UserService.DeleteUserAsync(user.Id);
                if (isDeleted)
                {
                    // Remove the user from the copied collection
                    userListUICopy.Remove(user);
                }
            }

            // Replace the original _userListUI collection with the modified copy
            _userListUI = userListUICopy;

            // Notify Blazor that the component's state has changed
            StateHasChanged();
        }

        // Nested class to represent a user in the UI
        protected class UserListUI
        {
            public Guid Id { get; set; }
            public string IdString { get; set; }
            public string Name { get; set; }
            public bool Selected { get; set; }
        }
    }
}
