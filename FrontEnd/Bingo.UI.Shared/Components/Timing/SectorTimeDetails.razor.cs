using Bingo.Common.DomainModel.MasterData;
using Bingo.Common.DomainModel.Setup;
using Bingo.Common.DomainModel.Timing;
using Bingo.UI.Shared.Services.MasterData;
using Bingo.UI.Shared.Services.Setup;
using Bingo.UI.Shared.Services.Timing;
using Microsoft.AspNetCore.Components;
using System.Runtime.Intrinsics.X86;

namespace Bingo.UI.Shared.Components.Timing
{
    public partial class SectorTimeDetails
    {
        [Inject] public ISectorTimeService SectorTimeService { get; set; }
        [Inject] public ISectorService SectorService { get; set; }
        [Inject] public ICompetitorService CompetitorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid Id { get; set; }

        // Properties for component's internal state.
        private SectorTime sectim { get; set; }
        private List<Sector> Sectors = new List<Sector>();
        private List<Competitor> Competitors = new List<Competitor>();

        private TimeSpan _netTime = TimeSpan.Zero;
        private long _netTimeTicks = 0;
        private TimeSpan _penaltyTime = TimeSpan.Zero;
        private TimeSpan _totTime = TimeSpan.Zero;
        private long _totalTimeTicks = 0;
        private decimal _avgSpeed = 0;

        Sector _localSector = new Sector();
        Competitor _localCompetitor = new Competitor();

        // Component initialization and sessionSector data fetch.
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                sectim = new SectorTime();
                sectim.Id = Guid.Empty;
                sectim.CompetitorId = Guid.Empty;
                sectim.SectorId = Guid.Empty;
                sectim.EntryTime = DateTime.MinValue;
                sectim.ExitTime = DateTime.MinValue;
                sectim.PenaltyTimeTicks = 0L;
                sectim.PenaltyPositions = 0;
                sectim.PenaltyNote = string.Empty;
                sectim.CreationTimeStamp = DateTime.MinValue;
                sectim.LastUpdateTimeStamp = DateTime.MinValue;
            }
            else
            {
                sectim = await SectorTimeService.GetSectorTimeByIdAsync(Id);
                Sectors = await SectorService.GetSectorsAsync();
                Competitors = await CompetitorService.GetCompetitorsAsync();
                RefreshTimes();
            }

            base.OnInitialized();
        }

        private void RefreshTimes(string newMessage = "")
        {
            if (sectim.ExitTime.TimeOfDay > TimeSpan.Zero && sectim.EntryTime.TimeOfDay > TimeSpan.Zero)
            {
                _netTime = sectim.ExitTime - sectim.EntryTime;
            }
            _netTimeTicks = _netTime.Ticks;
            _penaltyTime = TimeSpan.FromTicks(sectim.PenaltyTimeTicks ?? 0L);
            _totTime = _netTime + _penaltyTime;
            _totalTimeTicks = _totTime.Ticks;

            if (sectim.SectorId != default(Guid))
            {
                _localSector = Sectors.FirstOrDefault(sec => sec.Id == sectim.SectorId) ?? new Sector { Id = Guid.Empty, Length = 0, Name = "######" };
            }
            else
            {
                _localSector = new Sector { Id = Guid.Empty, Length = 0, Name = "######" };
            }
            if (_localSector.Length > 0 && _totTime.TotalHours > 0)
            {
                _avgSpeed = _localSector.Length ?? 0 / (decimal)_totTime.TotalHours;
            }
            else
            {
                _avgSpeed = 0;
            }

            if (sectim.CompetitorId != default(Guid))
            {
                _localCompetitor = Competitors.FirstOrDefault(comp => comp.Id == sectim.CompetitorId) ?? new Competitor { Id = Guid.Empty, Name1 = "######" };
            }
            else
            {
                _localCompetitor = new Competitor { Id = Guid.Empty, Name1 = "######" };
            }
        }

        // Handle form submissions
        private async Task HandleValidSubmit()
        {
            var result = new SectorTime();

            // Check if the Id is empty, then create a new sessionSector, otherwise edit the existing sessionSector
            if (Id == Guid.Empty)
            {
                result = await SectorTimeService.NewSectorTimeAsync(sectim);
            }
            else
            {
                result = await SectorTimeService.EditSectorTimeAsync(sectim);
            }

            // Update the SectorTime object and Id with the result
            sectim = result;
            Id = result.Id;

            // Update the URL with the new sessionSector ID to reflect the changes
            NavigationManager.NavigateTo($"/Timing/SectorTimeDetails/{Id}");
        }
    }
}