using Bingo.Common.DomainModel.MasterData;
using Bingo.UI.Shared.Services.MasterData;
using Microsoft.AspNetCore.Components;

namespace Bingo.UI.Shared.Components.MasterData
{
    public partial class CompetitorCategoryDetails
    {
        [Inject] public ICompetitorCategoryService CompetitorCategoryService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid Id { get; set; }

        // Properties for component's internal state.
        private CompetitorCategory cat { get; set; }

        // Component initialization and category data fetch.
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                cat = new CompetitorCategory();
                cat.Id = Guid.Empty;
                cat.Name = string.Empty;
                cat.CreationTimeStamp = DateTime.MinValue;
                cat.LastUpdateTimeStamp = DateTime.MinValue;
            }
            else
            {
                cat = await CompetitorCategoryService.GetCompetitorCategoryByIdAsync(Id);
            }

            base.OnInitialized();
        }

        // Handle form submissions
        private async Task HandleValidSubmit()
        {
            var result = new CompetitorCategory();

            // Check if the Id is empty, then create a new category, otherwise edit the existing category
            if (Id == Guid.Empty)
            {
                result = await CompetitorCategoryService.NewCompetitorCategoryAsync(cat);
            }
            else
            {
                result = await CompetitorCategoryService.EditCompetitorCategoryAsync(cat);
            }

            // Update the CompetitorCategory object and Id with the result
            cat = result;
            Id = result.Id;

            // Update the URL with the new category ID to reflect the changes
            NavigationManager.NavigateTo($"/Masterdata/CompetitorCategoryDetails/{Id}");
        }
    }
}