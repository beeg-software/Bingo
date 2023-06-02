using Bingo.Common.DomainModel.MasterData;
using Bingo.Common.DomainModel.Setup;
using Bingo.Common.DomainModel.Timing;
using Bingo.UI.Shared.Services.MasterData;
using Bingo.UI.Shared.Services.Setup;
using Bingo.UI.Shared.Services.Timing;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;
using System.Net.Http;
using System.Runtime.Intrinsics.X86;
using static MudBlazor.CategoryTypes;
using static MudBlazor.Colors;

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

        private string _localEntryTime = string.Empty;
        private string _localExitTime = string.Empty;
        private string _stringNetTime = string.Empty;
        private string _stringPenaltyTime = string.Empty;
        private string _stringTotTime = string.Empty;

        private TimeSpan _netTime = TimeSpan.Zero;
        private TimeSpan _penaltyTime = TimeSpan.Zero;
        private TimeSpan _totTime = TimeSpan.Zero;
        private decimal _avgSpeed = 0;

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
                        sectim.SectorId = _localSector.Id;
                    }
                }
                else
                {
                    _localSector = new Sector();
                }
            }
        }

        private Competitor _localCompetitor = new Competitor();
        private Competitor LocalCompetitor
        {
            get => _localCompetitor;
            set
            {
                if (value != null) {
                    if (_localCompetitor != value)
                    {
                        _localCompetitor = value;
                        sectim.CompetitorId = _localCompetitor.Id;
                    }
                }
                else
                {
                    _localCompetitor = new Competitor();
                }
            }
        }


        // Component initialization and sessionSector data fetch.
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                sectim = new SectorTime();
                sectim.Id = Guid.Empty;
                sectim.CompetitorId = Guid.Empty;
                sectim.SectorId = Guid.Empty;
                sectim.EntryTime = DateTime.Today;
                sectim.ExitTime = DateTime.Today;
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
            }

            _localEntryTime = sectim.EntryTime.ToString(@"HH\:mm\:ss\.fff");
            _localExitTime = sectim.ExitTime.ToString(@"HH\:mm\:ss\.fff");
            _penaltyTime = TimeSpan.FromTicks(sectim.PenaltyTimeTicks ?? 0L);
            _stringPenaltyTime = $"{_penaltyTime.Hours:00}:{_penaltyTime.Minutes:00}:{_penaltyTime.Seconds:00}.{_penaltyTime.Milliseconds:000}";


            RefreshTimes();

            base.OnInitialized();
        }

        private void RefreshTimes(string newMessage = "")
        {
            if (_localEntryTime != sectim.EntryTime.ToString(@"HH\:mm\:ss\.fff"))
            {
                if (DateTime.TryParse(sectim.EntryTime.ToString(@"yyyy\-MM\-dd") + " " + _localEntryTime, out DateTime tempDateTime))
                {
                    sectim.EntryTime = tempDateTime;
                }
                else
                {
                    _localEntryTime = sectim.EntryTime.ToString(@"HH\:mm\:ss\.fff");
                }
            }
            if (_localExitTime != sectim.ExitTime.ToString(@"HH\:mm\:ss\.fff"))
            {
                if (DateTime.TryParse(sectim.ExitTime.ToString(@"yyyy\-MM\-dd") + " " + _localExitTime, out DateTime tempDateTime))
                {
                    sectim.ExitTime = tempDateTime;
                }
                else
                {
                    _localExitTime = sectim.ExitTime.ToString(@"HH\:mm\:ss\.fff");
                }
            }
            if (_stringPenaltyTime != $"{_penaltyTime.Hours:00}:{_penaltyTime.Minutes:00}:{_penaltyTime.Seconds:00}.{_penaltyTime.Milliseconds:000}")
            {
                if (TimeSpan.TryParse(_stringPenaltyTime, CultureInfo.InvariantCulture, out TimeSpan tempTimeSpan))
                {
                    _penaltyTime = tempTimeSpan;
                    sectim.PenaltyTimeTicks = _penaltyTime.Ticks;
                }
                else
                {
                    _stringPenaltyTime = $"{_penaltyTime.Hours:00}:{_penaltyTime.Minutes:00}:{_penaltyTime.Seconds:00}.{_penaltyTime.Milliseconds:000}";
                }
            }

            if (sectim.ExitTime.TimeOfDay > TimeSpan.Zero && sectim.EntryTime.TimeOfDay > TimeSpan.Zero)
            {
                _netTime = sectim.ExitTime - sectim.EntryTime;
            }

            _totTime = _netTime + _penaltyTime;

            _stringNetTime = $"{_netTime.Hours:00}:{_netTime.Minutes:00}:{_netTime.Seconds:00}.{_netTime.Milliseconds:000}";
            _stringTotTime = $"{_totTime.Hours:00}:{_totTime.Minutes:00}:{_totTime.Seconds:00}.{_totTime.Milliseconds:000}";

            if (sectim.SectorId != default(Guid))
            {
                LocalSector = Sectors.FirstOrDefault(sec => sec.Id == sectim.SectorId) ?? new Sector { Id = Guid.Empty, Length = 0, Name = "######" };
            }
            else
            {
                LocalSector = new Sector { Id = Guid.Empty, Length = 0, Name = "######" };
            }
            if (LocalSector.Length > 0 && _totTime.TotalHours > 0)
            {
                _avgSpeed = LocalSector.Length ?? 0 / (decimal)_totTime.TotalHours;
            }
            else
            {
                _avgSpeed = 0;
            }

            if (sectim.CompetitorId != default(Guid))
            {
                LocalCompetitor = Competitors.FirstOrDefault(comp => comp.Id == sectim.CompetitorId) ?? new Competitor { Id = Guid.Empty, Name1 = "######" };
            }
            else
            {
                LocalCompetitor = new Competitor { Id = Guid.Empty, Name1 = "######" };
            }

            StateHasChanged();
        }

        private async Task<IEnumerable<Competitor>> SearchCompetitor(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Competitors;
            var filteredCompetitors = Competitors.Where(c => c.Number?.Contains(value) == true || c.Name1?.Contains(value) == true || c.Name2?.Contains(value) == true || c.Name3?.Contains(value) == true || c.Name4?.Contains(value) == true || c.Team?.Contains(value) == true);

            return filteredCompetitors;
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