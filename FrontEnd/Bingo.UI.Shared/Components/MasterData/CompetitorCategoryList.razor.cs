using Bingo.Common.DomainModel.MasterData;
using Bingo.UI.Shared.Services.MasterData;
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

        // Properties for component's internal state.
        protected List<CompetitorCategory> CompetitorCategories = new List<CompetitorCategory>();
        protected List<CompetitorCategoryListUI> _competitorCategoryListUI = new List<CompetitorCategoryListUI>();
        protected int _lineCount = 1;
        protected bool _selectAll;
        protected bool _isLoading = true;

        // Component initialization and user data fetch.
        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            CompetitorCategories = await CompetitorCategoryService.GetCompetitorCategoriesAsync();
            foreach (var cc in CompetitorCategories)
            {
                _competitorCategoryListUI.Add(new CompetitorCategoryListUI
                {
                    Id = cc.Id,
                    IdString = cc.Id.ToString(),
                    Name = cc.Name,
                    CreationTimeStamp = cc.CreationTimeStamp,
                    LastUpdateTimeStamp = cc.LastUpdateTimeStamp
                });
            }
            _isLoading = false;
        }

        // Export data to CSV file.
        protected async Task ExportToCSV()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\"Prog\",\"Nome\"");
            int progCount = 1;
            foreach (var cat in _competitorCategoryListUI.OrderBy(us => us.Id))
            {
                stringBuilder.AppendLine($"\"{progCount}\",\"{cat.Name}\"");
                progCount++;
            }
            await JSRuntime.InvokeVoidAsync("downloadCSV", "CompetitorCategoryList.csv", stringBuilder.ToString());
        }

        // Toggle all categories' selection and navigation methods.
        protected void ToggleSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var cat in _competitorCategoryListUI)
            {
                cat.Selected = _selectAll;
            }
        }

        protected void NavigateToNewEntity()
        {
            NavigationManager.NavigateTo($"/Masterdata/CompetitorCategoryDetails/{Guid.Empty}");
        }

        // Delete categories operations, with confirmation dialog.
        protected async Task ConfirmDeleteLines()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete the selected categories?");
            if (confirmed) await DeleteSelectedLines();
        }

        protected async Task DeleteSelectedLines()
        {
            var listCopy = _competitorCategoryListUI.ToList();
            var selectedLines = listCopy.Where(u => u.Selected).ToList();
            foreach (var line in selectedLines)
            {
                bool isDeleted = await CompetitorCategoryService.DeleteCompetitorCategoryAsync(line.Id);
                if (isDeleted) listCopy.Remove(line);
            }
            _competitorCategoryListUI = listCopy;
            StateHasChanged();
        }

        // Nested class to represent a category in the UI.
        protected class CompetitorCategoryListUI
        {
            public Guid Id { get; set; } = Guid.Empty;
            public string IdString { get; set; } = "";
            public string Name { get; set; } = "";
            public DateTime CreationTimeStamp { get; set; } = DateTime.MinValue;
            public DateTime LastUpdateTimeStamp { get; set; } = DateTime.MinValue;
            public bool Selected { get; set; } = false;
        }
    }
}
