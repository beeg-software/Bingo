using Bingo.Common.DomainModel.MasterData;
using Bingo.Common.DomainModel.Setup;
using Bingo.UI.Shared.Services.Setup;
using Microsoft.AspNetCore.Components;
using static System.Collections.Specialized.BitVector32;

namespace Bingo.UI.Shared.Components.Setup
{
    public partial class SessionSectorDetails
    {
        [Inject] public ISessionSectorService SessionSectorService { get; set; }
        [Inject] public ISectorService SectorService { get; set; }
        [Inject] public ISessionService SessionService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid Id { get; set; }

        // Properties for component's internal state.
        private SessionSector sese { get; set; }
        private List<Sector> Sectors = new List<Sector>();
        private List<Session> Sessions = new List<Session>();

        private Session _localSession = new Session();
        private Session LocalSession
        {
            get => _localSession;
            set
            {
                if (value != null)
                {
                    if (_localSession != value)
                    {
                        _localSession = value;
                        sese.SessionId = _localSession.Id;
                    }
                }
                else
                {
                    _localSession = new Session();
                }
            }
        }
        private Sector _localSector = new Sector();
        private Sector LocalSector
        {
            get => _localSector;
            set
            {
                if (value != null)
                {
                    if (_localSector != value)
                    {
                        _localSector = value;
                        sese.SectorId = _localSector.Id;
                    }
                }
                else
                {
                    _localSector = new Sector();
                }
            }
        }

        // Component initialization and sessionSector data fetch.
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                sese = new SessionSector();
                sese.Id = Guid.Empty;
                sese.SessionId = Guid.Empty;
                sese.SectorId = Guid.Empty;
                sese.RaceEnabled = false;
                sese.CreationTimeStamp = DateTime.MinValue;
                sese.LastUpdateTimeStamp = DateTime.MinValue;
            }
            else
            {
                sese = await SessionSectorService.GetSessionSectorByIdAsync(Id);
                Sessions = await SessionService.GetSessionsAsync();
                Sectors = await SectorService.GetSectorsAsync();
            }

            Refresh();
            base.OnInitialized();
        }

        private void Refresh()
        {
            if (sese.SessionId != default(Guid))
            {
                LocalSession = Sessions.FirstOrDefault(comp => comp.Id == sese.SessionId) ?? new Session { Id = Guid.Empty, Name = "######" };
            }
            else
            {
                LocalSession = new Session { Id = Guid.Empty, Name = "######" };
            }
            if (sese.SectorId != default(Guid))
            {
                LocalSector = Sectors.FirstOrDefault(comp => comp.Id == sese.SectorId) ?? new Sector { Id = Guid.Empty, Name = "######" };
            }
            else
            {
                LocalSector = new Sector { Id = Guid.Empty, Name = "######" };
            }
        }

        private async Task<IEnumerable<Session>> SearchSession(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Sessions;
            var filteredSessions = Sessions.Where(c => c.Name?.Contains(value) == true);

            return filteredSessions;
        }

        private async Task<IEnumerable<Sector>> SearchSector(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Sectors;
            var filteredSectors = Sectors.Where(c => c.Name?.Contains(value) == true || c.ImportName?.Contains(value) == true);

            return filteredSectors;
        }


        // Handle form submissions
        private async Task HandleValidSubmit()
        {
            var result = new SessionSector();

            // Check if the Id is empty, then create a new sessionSector, otherwise edit the existing sessionSector
            if (Id == Guid.Empty)
            {
                result = await SessionSectorService.NewSessionSectorAsync(sese);
            }
            else
            {
                result = await SessionSectorService.EditSessionSectorAsync(sese);
            }

            // Update the SessionSector object and Id with the result
            sese = result;
            Id = result.Id;

            // Update the URL with the new sessionSector ID to reflect the changes
            NavigationManager.NavigateTo($"/Setup/SessionSectorDetails/{Id}");
        }
    }
}