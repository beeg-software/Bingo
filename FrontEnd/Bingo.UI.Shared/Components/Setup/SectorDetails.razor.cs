using Bingo.Common.DomainModel.Setup;
using Bingo.UI.Shared.Services.Setup;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using static System.Collections.Specialized.BitVector32;

namespace Bingo.UI.Shared.Components.Setup
{
    public partial class SectorDetails
    {
        [Inject] public ISectorService SectorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid Id { get; set; }

        // Properties for component's internal state.
        private Sector sc { get; set; }

        private TimeSpan _minTime = TimeSpan.Zero;
        private string _stringMinTime = string.Empty;
        private TimeSpan _maxTime = TimeSpan.Zero;
        private string _stringMaxTime = string.Empty;

        // Component initialization and sector data fetch.
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                sc = new Sector();
                sc.Id = Guid.Empty;
                sc.Name = string.Empty;
                sc.ImportName = string.Empty;
                sc.Length = 0;
                sc.TargetAverageSpeed = 0;
                sc.MinTimeTicks = 0;
                sc.MaxTimeTicks = 0;
                sc.CreationTimeStamp = DateTime.MinValue;
                sc.LastUpdateTimeStamp = DateTime.MinValue;
            }
            else
            {
                sc = await SectorService.GetSectorByIdAsync(Id);
            }

            _minTime = TimeSpan.FromTicks(sc.MinTimeTicks ?? 0L);
            _stringMinTime = $"{_minTime.Hours:00}:{_minTime.Minutes:00}:{_minTime.Seconds:00}.{_minTime.Milliseconds:000}";
            _maxTime = TimeSpan.FromTicks(sc.MaxTimeTicks ?? 0L);
            _stringMaxTime = $"{_maxTime.Hours:00}:{_maxTime.Minutes:00}:{_maxTime.Seconds:00}.{_maxTime.Milliseconds:000}";

            RefreshTimes();
            base.OnInitialized();
        }

        private void RefreshTimes(string newMessage = "")
        {
            if (_stringMinTime != $"{_minTime.Hours:00}:{_minTime.Minutes:00}:{_minTime.Seconds:00}.{_minTime.Milliseconds:000}")
            {
                if (TimeSpan.TryParse(_stringMinTime, CultureInfo.InvariantCulture, out TimeSpan tempTimeSpan))
                {
                    _minTime = tempTimeSpan;
                    sc.MinTimeTicks = _minTime.Ticks;
                }
                else
                {
                    _stringMinTime = $"{_minTime.Hours:00}:{_minTime.Minutes:00}:{_minTime.Seconds:00}.{_minTime.Milliseconds:000}";
                }
            }
            if (_stringMaxTime != $"{_maxTime.Hours:00}:{_maxTime.Minutes:00}:{_maxTime.Seconds:00}.{_maxTime.Milliseconds:000}")
            {
                if (TimeSpan.TryParse(_stringMaxTime, CultureInfo.InvariantCulture, out TimeSpan tempTimeSpan))
                {
                    _maxTime = tempTimeSpan;
                    sc.MaxTimeTicks = _maxTime.Ticks;
                }
                else
                {
                    _stringMaxTime = $"{_maxTime.Hours:00}:{_maxTime.Minutes:00}:{_maxTime.Seconds:00}.{_maxTime.Milliseconds:000}";
                }
            }
        }

        // Handle form submissions
        private async Task HandleValidSubmit()
        {
            var result = new Sector();

            // Check if the Id is empty, then create a new sector, otherwise edit the existing sector
            if (Id == Guid.Empty)
            {
                result = await SectorService.NewSectorAsync(sc);
            }
            else
            {
                result = await SectorService.EditSectorAsync(sc);
            }

            // Update the Sector object and Id with the result
            sc = result;
            Id = result.Id;

            // Update the URL with the new sector ID to reflect the changes
            NavigationManager.NavigateTo($"/Setup/SectorDetails/{Id}");
        }
    }
}