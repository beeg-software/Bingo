using Bingo.Common.DomainModel.MasterData;
using Bingo.Common.DomainModel.Setup;
using Bingo.Common.DomainModel.Timing;
using Bingo.UI.Shared.Services.MasterData;
using Bingo.UI.Shared.Services.Setup;
using Bingo.UI.Shared.Services.Timing;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

namespace Bingo.UI.Shared.Components.Timing
{
    public partial class SectorTimeList
    {
        [Inject] public ISectorTimeService SectorTimeService { get; set; }
        [Inject] public ISectorService SectorService { get; set; }
        [Inject] public ICompetitorService CompetitorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        // Properties for component's internal state.
        protected List<SectorTime> SectorTimes = new List<SectorTime>();
        protected List<Competitor> Competitors = new List<Competitor>();
        protected List<Sector> Sectors = new List<Sector>();
        protected List<SectorTimeListUI> _sectorTimesListUI = new List<SectorTimeListUI>();
        protected int _lineCount = 1;
        protected bool _selectAll;
        protected bool _isLoading = true;

        // Component initialization and data fetch.
        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            SectorTimes = await SectorTimeService.GetSectorTimesAsync();
            Sectors = await SectorService.GetSectorsAsync();
            Competitors = await CompetitorService.GetCompetitorsAsync();
            foreach (var st in SectorTimes)
            {
                Sector _localSector = new Sector();
                Competitor _localCompetitor = new Competitor();

                TimeSpan _netTime = TimeSpan.Zero;
                TimeSpan _penaltyTime = TimeSpan.Zero;
                TimeSpan _totTime = TimeSpan.Zero;
                decimal _avgSpeed = 0;

                if (st.ExitTime.TimeOfDay != TimeSpan.Zero && st.EntryTime.TimeOfDay != TimeSpan.Zero)
                {
                    _netTime = st.ExitTime - st.EntryTime;
                }

                _penaltyTime = TimeSpan.FromTicks(st.PenaltyTimeTicks ?? 0L);
                _totTime = _netTime + _penaltyTime;

                if (st.SectorId != default(Guid))
                {
                    _localSector = Sectors?.FirstOrDefault(sec => sec.Id == st.SectorId) ?? new Sector { Id = Guid.Empty, Length = 0 };

                    if (_totTime.TotalHours > 0 && _localSector.Length > 0)
                    {
                        _avgSpeed = (_localSector.Length ?? 0) / (decimal)_totTime.TotalHours;
                    }
                    else
                    {
                        _avgSpeed = 0;
                    }
                }
                else
                {
                    _localSector = new Sector { Id = Guid.Empty, Length = 0 };
                    _avgSpeed = 0;
                }

                if (st.CompetitorId != default(Guid))
                {
                    _localCompetitor = Competitors.FirstOrDefault(comp => comp.Id == st.CompetitorId) ?? new Competitor { Id = Guid.Empty, Number = "######" };
                }
                else
                {
                    _localCompetitor = new Competitor { Id = Guid.Empty, Number = "######" };
                }

                _sectorTimesListUI.Add(new SectorTimeListUI
                {
                    Id = st.Id,
                    IdString = st.Id.ToString(),
                    CompetitorId = st.CompetitorId ?? Guid.Empty,
                    CompetitorNumber = _localCompetitor.Number,
                    SectorId = st.SectorId ?? Guid.Empty,
                    SectorName = _localSector.Name,
                    EntryTime = st.EntryTime,
                    ExitTime = st.ExitTime,
                    PenaltyTimeTicks = st.PenaltyTimeTicks ?? 0L,
                    PenaltyPositions = st.PenaltyPositions ?? 0,
                    PenaltyNote = st.PenaltyNote ?? string.Empty,
                    NetTime = _netTime,
                    PenaltyTime = _penaltyTime,
                    TotalTime = _totTime,
                    AchievedAverageSpeed = _avgSpeed,
                    CreationTimeStamp = st.CreationTimeStamp,
                    LastUpdateTimeStamp = st.LastUpdateTimeStamp
                });
            }
            _isLoading = false;
        }

        // Export data to CSV file.
        protected async Task ExportToCSV()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\"Prog\",\"Competitor\",\"Sector\",\"EntryTime\",\"ExitTime\",\"NetTime\",\"PenaltyTime\",\"TotalTime\",\"AchievedAverageSpeed\",\"PenaltyPositions\",\"PenaltyNote\"");
            int progCount = 1;
            foreach (var sect in _sectorTimesListUI.OrderBy(us => us.Id))
            {
                stringBuilder.AppendLine($"\"{progCount}\",\"{sect.CompetitorNumber}\",\"{sect.SectorName}\",\"{sect.EntryTime.ToString()}\",\"{sect.ExitTime.ToString()}\",\"{sect.NetTime.ToString()}\",\"{sect.PenaltyTime.ToString()}\",\"{sect.TotalTime.ToString()}\",\"{sect.AchievedAverageSpeed.ToString()}\",\"{sect.PenaltyPositions.ToString()}\",\"{sect.PenaltyNote}\"");
                progCount++;
            }
            await JSRuntime.InvokeVoidAsync("downloadCSV", "SectorTimeList.csv", stringBuilder.ToString());
        }

        // Toggle all sector times' selection and navigation methods.
        protected void ToggleSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var sect in _sectorTimesListUI)
            {
                sect.Selected = _selectAll;
            }
        }

        protected void NavigateToNewEntity()
        {
            NavigationManager.NavigateTo($"/Timing/SectorTimeDetails/{Guid.Empty}");
        }

        // Delete sector times operations, with confirmation dialog.
        protected async Task ConfirmDeleteLines()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the selected sector times?");
            if (confirmed) await DeleteSelectedLines();
        }

        protected async Task DeleteSelectedLines()
        {
            var listCopy = _sectorTimesListUI.ToList();
            var selectedLines = listCopy.Where(u => u.Selected).ToList();
            foreach (var line in selectedLines)
            {
                bool isDeleted = await SectorTimeService.DeleteSectorTimeAsync(line.Id);
                if (isDeleted) listCopy.Remove(line);
            }
            _sectorTimesListUI = listCopy;
            StateHasChanged();
        }

        // Nested class to represent a sector time in the UI.
        protected class SectorTimeListUI
        {
            public Guid Id { get; set; } = Guid.Empty;
            public string IdString { get; set; } = "";
            public Guid CompetitorId { get; set; } = Guid.Empty;
            public string CompetitorNumber { get; set; } = "";
            public Guid SectorId { get; set; } = Guid.Empty;
            public string SectorName { get; set; } = "";
            public DateTime EntryTime { get; set; } = DateTime.MinValue;
            public DateTime ExitTime { get; set; } = DateTime.MinValue;
            public TimeSpan NetTime { get; set; } = TimeSpan.Zero;
            public long PenaltyTimeTicks { get; set; } = 0;
            public TimeSpan PenaltyTime { get; set; } = TimeSpan.Zero;
            public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;
            public decimal AchievedAverageSpeed { get; set; } = 0;
            public Int32 PenaltyPositions { get; set; } = 0;
            public string PenaltyNote { get; set; } = "";
            public DateTime CreationTimeStamp { get; set; } = DateTime.MinValue;
            public DateTime LastUpdateTimeStamp { get; set; } = DateTime.MinValue;
            public bool Selected { get; set; } = false;
        }
    }
}
