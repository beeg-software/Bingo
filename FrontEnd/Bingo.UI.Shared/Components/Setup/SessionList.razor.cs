using Bingo.Common.DomainModel.Setup;
using Bingo.UI.Shared.Services.Setup;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

namespace Bingo.UI.Shared.Components.Setup
{
    public partial class SessionList
    {
        [Inject] public ISessionService SessionService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        // Properties for component's internal state.
        protected List<Session> Sessions = new List<Session>();
        protected List<SessionListUI> _sessionListUI = new List<SessionListUI>();
        protected int _lineCount = 1;
        protected bool _selectAll;
        protected bool _isLoading = true;

        // Component initialization and session data fetch.
        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            Sessions = await SessionService.GetSessionsAsync();
            foreach (var ss in Sessions)
            {
                _sessionListUI.Add(new SessionListUI
                {
                    Id = ss.Id,
                    IdString = ss.Id.ToString(),
                    Name = ss.Name,
                    CreationTimeStamp = ss.CreationTimeStamp,
                    LastUpdateTimeStamp = ss.LastUpdateTimeStamp
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
            foreach (var ss in _sessionListUI.OrderBy(us => us.Id))
            {
                stringBuilder.AppendLine($"\"{progCount}\",\"{ss.Name}\"");
                progCount++;
            }
            await JSRuntime.InvokeVoidAsync("downloadCSV", "SessionList.csv", stringBuilder.ToString());
        }

        // Toggle all sessions' selection and navigation methods.
        protected void ToggleSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var ss in _sessionListUI)
            {
                ss.Selected = _selectAll;
            }
        }

        protected void NavigateToNewEntity()
        {
            NavigationManager.NavigateTo($"/Masterdata/SessionDetails/{Guid.Empty}");
        }

        // Delete sessions operations, with confirmation dialog.
        protected async Task ConfirmDeleteLines()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the selected sessions?");
            if (confirmed) await DeleteSelectedLines();
        }

        protected async Task DeleteSelectedLines()
        {
            var listCopy = _sessionListUI.ToList();
            var selectedLines = listCopy.Where(u => u.Selected).ToList();
            foreach (var line in selectedLines)
            {
                bool isDeleted = await SessionService.DeleteSessionAsync(line.Id);
                if (isDeleted) listCopy.Remove(line);
            }
            _sessionListUI = listCopy;
            StateHasChanged();
        }

        // Nested class to represent a session in the UI.
        protected class SessionListUI
        {
            public Guid Id { get; set; } = Guid.Empty;
            public string IdString { get; set; } = "";
            public string Name { get; set; } = "";
            public DateTime CreationTimeStamp { get; set; } = DateTime.MinValue;
            public DateTime LastUpdateTimeStamp { get; set; } = DateTime.MinValue;
            public bool Selected { get; set; } = false;
        }
    }
}
