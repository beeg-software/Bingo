using Bingo.Common.DomainModel.MasterData;
using Bingo.UI.Shared.Services.MasterData;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

namespace Bingo.UI.Shared.Components.MasterData
{
    public partial class CompetitorList
    {
        [Inject] public ICompetitorService CompetitorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        // Properties for component's internal state.
        protected List<Competitor> Competitors = new List<Competitor>();
        protected List<CompetitorListUI> _competitorListUI = new List<CompetitorListUI>();
        protected int _lineCount = 1;
        protected bool _selectAll;
        protected bool _isLoading = true;

        // Component initialization and competitor data fetch.
        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            Competitors = await CompetitorService.GetCompetitorsAsync();
            foreach (var co in Competitors)
            {
                _competitorListUI.Add(new CompetitorListUI
                {
                    Id = co.Id,
                    IdString = co.Id.ToString(),
                    Number = co.Number,
                    Name1 = co.Name1,
                    Name2 = co.Name2,
                    Name3 = co.Name3,
                    Name4 = co.Name4,
                    Nationality = co.Nationality,
                    Team = co.Team,
                    Boat = co.Boat,
                    Engine = co.Engine,
                    Status = co.Status,
                    CreatedTimeStamp = co.CreationTimeStamp
                });
            }
            _isLoading = false;
        }

        // Export data to CSV file.
        protected async Task ExportToCSV()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\"Prog\",\"Number\",\"Nome1\",\"Nome2\",\"Nome3\",\"Nome4\",\"Nationality\",\"Team\",\"Boat\",\"Engine\",\"Status\"");
            int progCount = 1;
            foreach (var competitor in _competitorListUI.OrderBy(us => us.Id))
            {
                stringBuilder.AppendLine($"\"{progCount}\",\"{competitor.Number}\",\"{competitor.Name1}\",\"{competitor.Name2}\",\"{competitor.Name3}\",\"{competitor.Name4}\",\"{competitor.Nationality}\",\"{competitor.Team}\",\"{competitor.Boat}\",\"{competitor.Engine}\",\"{competitor.Status}\"");
                progCount++;
            }
            await JSRuntime.InvokeVoidAsync("downloadCSV", "CompetitorList.csv", stringBuilder.ToString());
        }

        // Toggle all competitors' selection and navigation methods.
        protected void ToggleSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var competitor in _competitorListUI)
            {
                competitor.Selected = _selectAll;
            }
        }

        protected void NavigateToNewEntity()
        {
            NavigationManager.NavigateTo($"/Masterdata/CompetitorDetails/{Guid.Empty}");
        }

        // Delete competitors operations, with confirmation dialog.
        protected async Task ConfirmDeleteLines()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the selected competitors?");
            if (confirmed) await DeleteSelectedLines();
        }

        protected async Task DeleteSelectedLines()
        {
            var listCopy = _competitorListUI.ToList();
            var selectedLines = listCopy.Where(u => u.Selected).ToList();
            foreach (var line in selectedLines)
            {
                bool isDeleted = await CompetitorService.DeleteCompetitorAsync(line.Id);
                if (isDeleted) listCopy.Remove(line);
            }
            _competitorListUI = listCopy;
            StateHasChanged();
        }

        // Nested class to represent a competitor in the UI.
        protected class CompetitorListUI
        {
            public Guid Id { get; set; }
            public string IdString { get; set; }
            public string Number { get; set; }
            public string Name1 { get; set; }
            public string Name2 { get; set; } = "";
            public string Name3 { get; set; } = "";
            public string Name4 { get; set; } = "";
            public string Nationality { get; set; } = "";
            public string Team { get; set; } = "";
            public string Boat { get; set; } = "";
            public string Engine { get; set; } = "";
            public string Status { get; set; } = "";
            public DateTime CreatedTimeStamp { get; set; }
            public bool Selected { get; set; }
        }
    }
}
