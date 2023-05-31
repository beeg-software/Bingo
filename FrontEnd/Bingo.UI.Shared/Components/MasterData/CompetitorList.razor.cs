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

        protected List<Competitor> Competitor = new List<Competitor>();
        protected List<CompetitorListUI> _competitorListUI = new List<CompetitorListUI>();
        protected int _progCount = 1;
        protected bool _selectAll;
        protected bool _isLoading = true;


        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            Competitor = await CompetitorService.GetCompetitorsAsync();

            foreach (var co in Competitor)
            {
                _competitorListUI.Add(new CompetitorListUI
                {
                    Id = co.Id,
                    IdString = co.Id.ToString(),
                    Name1 = co.Name1
                });
            }

            _isLoading = false;
        }

        protected async Task ExportToCSV()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("\"Prog\",\"Nome1\"");

            int progCount = 1;
            foreach (var competitor in _competitorListUI.OrderBy(us => us.Id))
            {
                stringBuilder.AppendLine($"\"{progCount}\",\"{competitor.Name1}\"");
                progCount++;
            }

            var csvData = stringBuilder.ToString();

            await JSRuntime.InvokeVoidAsync("downloadCSV", "CompetitorList.csv", csvData);
        }

        protected void ToggleSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var competitor in _competitorListUI)
            {
                competitor.Selected = _selectAll;
            }
        }

        protected void NavigateToNewCompetitor()
        {
            NavigationManager.NavigateTo($"/Masterdata/CompetitorDetails/{Guid.Empty}");
        }

        protected async Task ConfirmDeleteCompetitors()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the selected competitors?");
            if (confirmed)
            {
                await DeleteSelectedCompetitors();
            }
        }

        protected async Task DeleteSelectedCompetitors()
        {
            var selectedCompetitors = _competitorListUI.Where(u => u.Selected).ToList();

            var _competitorListUICopy = _competitorListUI.ToList();

            foreach (var competitor in selectedCompetitors)
            {
                bool isDeleted = await CompetitorService.DeleteCompetitorAsync(competitor.Id);
                if (isDeleted)
                {
                    _competitorListUICopy.Remove(competitor);
                }
            }

            _competitorListUI = _competitorListUICopy;

            StateHasChanged();
        }

        protected class CompetitorListUI
        {
            public Guid Id { get; set; }
            public string IdString { get; set; }
            public string Name1 { get; set; }
            public bool Selected { get; set; }
        }
    }
}
