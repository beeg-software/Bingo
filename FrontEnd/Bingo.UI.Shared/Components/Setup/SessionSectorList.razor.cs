using Bingo.Common.DomainModel.MasterData;
using Bingo.Common.DomainModel.Setup;
using Bingo.UI.Shared.Services.Setup;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

namespace Bingo.UI.Shared.Components.Setup
{
    public partial class SessionSectorList
    {
        [Inject] public ISessionSectorService SessionSectorService { get; set; }
        [Inject] public ISessionService SessionService { get; set; }
        [Inject] public ISectorService SectorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        // Properties for component's internal state.
        protected List<SessionSector> SessionSectors = new List<SessionSector>();
        protected List<Sector> Sectors = new List<Sector>();
        protected List<Session> Sessions = new List<Session>();
        protected List<SessionSectorListUI> _sessionSectorListUI = new List<SessionSectorListUI>();
        protected int _lineCount = 1;
        protected bool _selectAll;
        protected bool _isLoading = true;

        // Component initialization and sessionSector data fetch.
        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            SessionSectors = await SessionSectorService.GetSessionSectorsAsync();
            Sessions = await SessionService.GetSessionsAsync();
            Sectors = await SectorService.GetSectorsAsync();

            foreach (var sese in SessionSectors)
            {
                Session _localSession = new Session();
                Sector _localSector = new Sector();

                if (sese.SessionId != default(Guid))
                {
                    _localSession = Sessions.FirstOrDefault(comp => comp.Id == sese.SessionId) ?? new Session { Id = Guid.Empty, Name = "######" };
                }
                else
                {
                    _localSession = new Session { Id = Guid.Empty, Name = "######" };
                }

                if (sese.SectorId != default(Guid))
                {
                    _localSector = Sectors.FirstOrDefault(comp => comp.Id == sese.SectorId) ?? new Sector { Id = Guid.Empty, Name = "######" };
                }
                else
                {
                    _localSector = new Sector { Id = Guid.Empty, Name = "######" };
                }

                _sessionSectorListUI.Add(new SessionSectorListUI
                {
                    Id = sese.Id,
                    IdString = sese.Id.ToString(),
                    SessionId = sese.SessionId ?? Guid.Empty,
                    SessionName = _localSession.Name,
                    SectorId = sese.SectorId ?? Guid.Empty,
                    SectorName = _localSector.Name,
                    RaceEnabled = sese.RaceEnabled,
                    CreationTimeStamp = sese.CreationTimeStamp,
                    LastUpdateTimeStamp = sese.LastUpdateTimeStamp
                });
            }
            _isLoading = false;
        }

        // Export data to CSV file.
        protected async Task ExportToCSV()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\"Prog\",\"Session\",\"Sector\",\"RaceEnabled\"");
            int progCount = 1;
            foreach (var sese in _sessionSectorListUI.OrderBy(us => us.Id))
            {
                stringBuilder.AppendLine($"\"{progCount}\",\"{sese.SessionName}\",\"{sese.SectorName}\",\"{sese.RaceEnabled}\"");
                progCount++;
            }
            await JSRuntime.InvokeVoidAsync("downloadCSV", "SessionSectorList.csv", stringBuilder.ToString());
        }

        // Toggle all sessionSectors' selection and navigation methods.
        protected void ToggleSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var ss in _sessionSectorListUI)
            {
                ss.Selected = _selectAll;
            }
        }

        protected void NavigateToNewEntity()
        {
            NavigationManager.NavigateTo($"/Masterdata/SessionSectorDetails/{Guid.Empty}");
        }

        // Delete sessionSectors operations, with confirmation dialog.
        protected async Task ConfirmDeleteLines()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the selected session-sectors?");
            if (confirmed) await DeleteSelectedLines();
        }

        protected async Task DeleteSelectedLines()
        {
            var listCopy = _sessionSectorListUI.ToList();
            var selectedLines = listCopy.Where(u => u.Selected).ToList();
            foreach (var line in selectedLines)
            {
                bool isDeleted = await SessionSectorService.DeleteSessionSectorAsync(line.Id);
                if (isDeleted) listCopy.Remove(line);
            }
            _sessionSectorListUI = listCopy;
            StateHasChanged();
        }

        // Nested class to represent a sessionSector in the UI.
        protected class SessionSectorListUI
        {
            public Guid Id { get; set; } = Guid.Empty;
            public string IdString { get; set; } = "";
            public Guid SessionId { get; set; } = Guid.Empty;
            public string SessionName { get; set; } = "";
            public Guid SectorId { get; set; } = Guid.Empty;
            public string SectorName { get; set; } = "";
            public bool RaceEnabled { get; set; } = false;
            public DateTime CreationTimeStamp { get; set; } = DateTime.MinValue;
            public DateTime LastUpdateTimeStamp { get; set; } = DateTime.MinValue;
            public bool Selected { get; set; } = false;
        }
    }
}
