using Bingo.Common.DomainModel.MasterData;
using Bingo.UI.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

namespace Bingo.UI.Shared.Components.MasterData
{
    public partial class CompetitorCategoryList
    {
        [Inject] public ICompetitorCategoryService CompetitorCategoryService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        protected List<CompetitorCategory> Categories = new List<CompetitorCategory>();
        protected List<CompetitorCategoryListUI> _competitorCategoryListUI = new List<CompetitorCategoryListUI>();
        protected int _progCount = 1;
        protected bool _selectAll;
        protected bool _isLoading = true;


        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            Categories = await CompetitorCategoryService.GetCompetitorCategoriesAsync();

            foreach (var ca in Categories)
            {
                _competitorCategoryListUI.Add(new CompetitorCategoryListUI
                {
                    Id = ca.Id,
                    IdString = ca.Id.ToString(),
                    Name = ca.Name
                });
            }

            _isLoading = false;
        }

        protected async Task ExportToCSV()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("\"Prog\",\"Nome\"");

            int progCount = 1;
            foreach (var category in _competitorCategoryListUI.OrderBy(us => us.Id))
            {
                stringBuilder.AppendLine($"\"{progCount}\",\"{category.Name}\"");
                progCount++;
            }

            var csvData = stringBuilder.ToString();

            await JSRuntime.InvokeVoidAsync("downloadCSV", "CompetitorCategoryList.csv", csvData);
        }

        protected void ToggleSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var user in _competitorCategoryListUI)
            {
                user.Selected = _selectAll;
            }
        }

        protected void NavigateToNewCompetitorCategory()
        {
            NavigationManager.NavigateTo($"/Masterdata/CompetitorCategoryDetails/{Guid.Empty}");
        }

        protected async Task ConfirmDeleteCompetitorCategories()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the selected categories?");
            if (confirmed)
            {
                await DeleteSelectedCompetitorCategories();
            }
        }

        protected async Task DeleteSelectedCompetitorCategories()
        {
            var selectedCompetitorCategory = _competitorCategoryListUI.Where(u => u.Selected).ToList();

            var _competitorCategoryListUICopy = _competitorCategoryListUI.ToList();

            foreach (var competitorCategory in selectedCompetitorCategory)
            {
                bool isDeleted = await CompetitorCategoryService.DeleteCompetitorCategoryAsync(competitorCategory.Id);
                if (isDeleted)
                {
                    _competitorCategoryListUICopy.Remove(competitorCategory);
                }
            }

            _competitorCategoryListUI = _competitorCategoryListUICopy;

            StateHasChanged();
        }

        protected class CompetitorCategoryListUI
        {
            public Guid Id { get; set; }
            public string IdString { get; set; }
            public string Name { get; set; }
            public bool Selected { get; set; }
        }
    }
}
