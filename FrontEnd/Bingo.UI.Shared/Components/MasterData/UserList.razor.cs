using Bingo.Common.DomainModel.MasterData;
using Bingo.UI.Shared.Services.MasterData;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

namespace Bingo.UI.Shared.Components.MasterData
{
    public partial class UserList
    {
        [Inject] public IUserService UserService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        // Properties for component's internal state.
        protected List<User> Users = new List<User>();
        protected List<UserListUI> _userListUI = new List<UserListUI>();
        protected int _lineCount = 1;
        protected bool _selectAll;
        protected bool _isLoading = true;

        // Component initialization and user data fetch.
        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            Users = await UserService.GetUsersAsync();
            foreach (var us in Users)
            {
                _userListUI.Add(new UserListUI
                {
                    Id = us.Id,
                    IdString = us.Id.ToString(),
                    Name = us.Name,
                    CreatedTimeStamp = us.CreationTimeStamp
                });
            }
            _isLoading = false;
        }

        // Export data to CSV file.
        protected async Task ExportToCSV()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\"Prog\",\"Nome\"");
            int progCount = 1;
            foreach (var user in _userListUI.OrderBy(us => us.Id))
            {
                stringBuilder.AppendLine($"\"{progCount}\",\"{user.Name}\"");
                progCount++;
            }
            await JSRuntime.InvokeVoidAsync("downloadCSV", "UserList.csv", stringBuilder.ToString());
        }

        // Toggle all users' selection and navigation methods.
        protected void ToggleSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var user in _userListUI)
            {
                user.Selected = _selectAll;
            }
        }

        protected void NavigateToNewEntity()
        {
            NavigationManager.NavigateTo($"/Masterdata/UserDetails/{Guid.Empty}");
        }

        // Delete users operations, with confirmation dialog.
        protected async Task ConfirmDeleteLines()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the selected users?");
            if (confirmed) await DeleteSelectedLines();
        }

        protected async Task DeleteSelectedLines()
        {
            var listCopy = _userListUI.ToList();
            var selectedLines = listCopy.Where(u => u.Selected).ToList();
            foreach (var line in selectedLines)
            {
                bool isDeleted = await UserService.DeleteUserAsync(line.Id);
                if (isDeleted) listCopy.Remove(line);
            }
            _userListUI = listCopy;
            StateHasChanged();
        }

        // Nested class to represent a user in the UI.
        protected class UserListUI
        {
            public Guid Id { get; set; }
            public string IdString { get; set; }
            public string Name { get; set; }
            public DateTime CreatedTimeStamp { get; set; }
            public bool Selected { get; set; }
        }
    }
}
