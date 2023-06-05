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
    public partial class TimingSheet
    {
        [Inject] public ISectorTimeService SectorTimeService { get; set; }
        [Inject] public ISectorService SectorService { get; set; }
        [Inject] public ICompetitorService CompetitorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        // Properties for component's internal state.
        private List<Sector> Sectors = new List<Sector>();
        private List<SectorTime> SectorTimes = new List<SectorTime>();
        private List<Competitor> Competitors = new List<Competitor>();

        private List<TimingSheetListUI> _timingSheetListUI = new List<TimingSheetListUI>();

        protected bool _isLoading = true;

        // Component initialization and session data fetch.
        protected override async Task OnInitializedAsync()
        {
            SectorTimes = await SectorTimeService.GetSectorTimesAsync();
            Sectors = await SectorService.GetSectorsAsync();
            Sectors = Sectors.OrderBy(s => s.ImportName).ToList();
            Competitors = await CompetitorService.GetCompetitorsAsync();

            foreach (var comp in Competitors)
            {
                List<SectorTime> _localSectorTimes = new List<SectorTime>();
                foreach (var sec in Sectors)
                {
                    SectorTime tempST = new SectorTime();
                    if (SectorTimes.Any(st => st.CompetitorId == comp.Id && st.SectorId == sec.Id))
                    {
                        tempST = SectorTimes.FirstOrDefault(st => st.CompetitorId == comp.Id && st.SectorId == sec.Id);
                        _localSectorTimes.Add(tempST);
                    }
                }

                _timingSheetListUI.Add(new TimingSheetListUI
                {
                    CompetitorId = comp.Id,
                    CompetitorNumber = comp.Number,
                    CompetitorImportNumber = comp.ImportNumber,
                    SectorsCollection = _localSectorTimes
                });
            }

            _timingSheetListUI = _timingSheetListUI.OrderBy(sta => sta.CompetitorImportNumber.Length).ThenBy(sta => sta.CompetitorImportNumber).ToList();
            _isLoading = false;
        }


        class TimingSheetListUI
        {
            public Guid CompetitorId { get; set; }
            public string CompetitorNumber { get; set; }
            public string CompetitorImportNumber { get; set; }
            public List<SectorTime> SectorsCollection { get; set; }
        }
    }
}
