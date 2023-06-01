using Bingo.Common.DomainModel.Setup;
using Bingo.UI.Shared.Services.Setup;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

namespace Bingo.UI.Shared.Components.Setup
{
    public partial class SectorList
    {
        [Inject] public ISectorService SectorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        // Properties for component's internal state.
        protected List<Sector> Sectors = new List<Sector>();
        protected List<SectorListUI> _sectorListUI = new List<SectorListUI>();
        protected int _lineCount = 1;
        protected bool _selectAll;
        protected bool _isLoading = true;

        // Component initialization and sector data fetch.
        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            Sectors = await SectorService.GetSectorsAsync();
            foreach (var ss in Sectors)
            {
                TimeSpan _minTime = TimeSpan.Zero;
                TimeSpan _maxTime = TimeSpan.Zero;

                _minTime = TimeSpan.FromTicks(ss.MinTimeTicks ?? 0L);
                _maxTime = TimeSpan.FromTicks(ss.MaxTimeTicks ?? 0L);

                _sectorListUI.Add(new SectorListUI
                {
                    Id = ss.Id,
                    IdString = ss.Id.ToString(),
                    Name = ss.Name,
                    ImportName = ss.Name,
                    Length = ss.Length ?? 0,
                    TargetAverageSpeed = ss.TargetAverageSpeed ?? 0,
                    MinTimeTicks = ss.MinTimeTicks ?? 0L,
                    MinTime = _minTime,
                    MaxTimeTicks = ss.MaxTimeTicks ?? 0L,
                    MaxTime = _maxTime,
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
            stringBuilder.AppendLine("\"Prog\",\"Nome\",\"ImportName\",\"Length\",\"TargetAverageSpeed\",\"MinTime\",\"MaxTime\"");
            int progCount = 1;
            foreach (var ss in _sectorListUI.OrderBy(us => us.Id))
            {
                stringBuilder.AppendLine($"\"{progCount}\",\"{ss.Name}\",\"{ss.ImportName}\",\"{ss.Length.ToString("F2")}\",\"{ss.TargetAverageSpeed.ToString("F2")}\",\"{ss.MinTime.ToString(@"hh\:mm\:ss\.fff")}\",\"{ss.MaxTime.ToString(@"hh\:mm\:ss\.fff")}\"");
                progCount++;
            }
            await JSRuntime.InvokeVoidAsync("downloadCSV", "SectorList.csv", stringBuilder.ToString());
        }

        // Toggle all sectors' selection and navigation methods.
        protected void ToggleSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var ss in _sectorListUI)
            {
                ss.Selected = _selectAll;
            }
        }

        protected void NavigateToNewEntity()
        {
            NavigationManager.NavigateTo($"/Setup/SectorDetails/{Guid.Empty}");
        }

        // Delete sectors operations, with confirmation dialog.
        protected async Task ConfirmDeleteLines()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the selected sectors?");
            if (confirmed) await DeleteSelectedLines();
        }

        protected async Task DeleteSelectedLines()
        {
            var listCopy = _sectorListUI.ToList();
            var selectedLines = listCopy.Where(u => u.Selected).ToList();
            foreach (var line in selectedLines)
            {
                bool isDeleted = await SectorService.DeleteSectorAsync(line.Id);
                if (isDeleted) listCopy.Remove(line);
            }
            _sectorListUI = listCopy;
            StateHasChanged();
        }

        // Nested class to represent a sector in the UI.
        protected class SectorListUI
        {
            public Guid Id { get; set; } = Guid.Empty;
            public string IdString { get; set; } = "";
            public string Name { get; set; } = "";
            public string ImportName { get; set; } = "";
            public decimal Length { get; set; } = 0;
            public decimal TargetAverageSpeed { get; set; } = 0;
            public long MinTimeTicks { get; set; } = 0L;
            public TimeSpan MinTime { get; set; } = TimeSpan.Zero;
            public long MaxTimeTicks { get; set; } = 0L;
            public TimeSpan MaxTime { get; set; } = TimeSpan.Zero;
            public DateTime CreationTimeStamp { get; set; } = DateTime.MinValue;
            public DateTime LastUpdateTimeStamp { get; set; } = DateTime.MinValue;
            public bool Selected { get; set; } = false;
        }
    }
}
