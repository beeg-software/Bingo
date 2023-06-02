using Bingo.Common.DomainModel.MasterData;
using Bingo.Common.DomainModel.Setup;
using Bingo.UI.Shared.Services.MasterData;
using Bingo.UI.Shared.Services.Setup;
using Microsoft.AspNetCore.Components;
using static System.Collections.Specialized.BitVector32;

namespace Bingo.UI.Shared.Components.MasterData
{
    public partial class CompetitorDetails
    {
        [Inject] public ICompetitorService CompetitorService { get; set; }
        [Inject] public ICompetitorCategoryService CompetitorCategoryService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid Id { get; set; }

        // Properties for component's internal state.
        private Competitor comp { get; set; }
        private List<CompetitorCategory> CompetitorCategories = new List<CompetitorCategory>();

        private CompetitorCategory _localCompetitorCategory = new CompetitorCategory();
        private CompetitorCategory LocalCompetitorCategory
        {
            get => _localCompetitorCategory;
            set
            {
                if (value != null)
                {
                    if (_localCompetitorCategory != value)
                    {
                        _localCompetitorCategory = value;
                        comp.CompetitorCategoryId = _localCompetitorCategory.Id;
                    }
                }
                else
                {
                    _localCompetitorCategory = new CompetitorCategory();
                }
            }
        }

        // Component initialization and competitor data fetch.
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                comp = new Competitor();
                comp.Id = Guid.Empty;
                comp.Name1 = string.Empty;
                comp.Name2 = string.Empty;
                comp.Name3 = string.Empty;
                comp.Name4 = string.Empty;
                comp.Nationality = string.Empty;
                comp.Team = string.Empty;
                comp.Boat = string.Empty;
                comp.Engine = string.Empty;
                comp.Status = string.Empty;
                comp.CreationTimeStamp = DateTime.MinValue;
                comp.LastUpdateTimeStamp = DateTime.MinValue;
            }
            else
            {
                comp = await CompetitorService.GetCompetitorByIdAsync(Id);
                CompetitorCategories = await CompetitorCategoryService.GetCompetitorCategoriesAsync();
            }

            base.OnInitialized();
        }

        private async Task<IEnumerable<CompetitorCategory>> SearchCompetitorCategory(string value)
        {
            if (string.IsNullOrEmpty(value))
                return CompetitorCategories;
            var filteredSectors = CompetitorCategories.Where(c => c.Name?.Contains(value) == true);

            return filteredSectors;
        }

        // Handle form submissions
        private async Task HandleValidSubmit()
        {
            var result = new Competitor();

            // Check if the Id is empty, then create a new competitor, otherwise edit the existing competitor
            if (Id == Guid.Empty)
            {
                result = await CompetitorService.NewCompetitorAsync(comp);
            }
            else
            {
                result = await CompetitorService.EditCompetitorAsync(comp);
            }

            // Update the Competitor object and Id with the result
            comp = result;
            Id = result.Id;

            // Update the URL with the new competitor ID to reflect the changes
            NavigationManager.NavigateTo($"/Masterdata/CompetitorDetails/{Id}");
        }
    }
}