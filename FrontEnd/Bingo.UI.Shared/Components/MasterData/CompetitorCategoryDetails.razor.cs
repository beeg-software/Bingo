using Bingo.Common.DomainModel.MasterData;
using Bingo.UI.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace Bingo.UI.Shared.Components.MasterData
{
    public partial class CompetitorCategoryDetails
    {
        [Inject] public ICompetitorCategoryService CompetitorCategoryService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }


        [Parameter] public Guid Id { get; set; }
        private CompetitorCategory cat { get; set; }

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

        private async Task HandleValidSubmit()
        {
            var result = new CompetitorCategory();

            if (Id == Guid.Empty)
            {
                result = await CompetitorCategoryService.NewCompetitorCategoryAsync(cat);
            }
            else
            {
                result = await CompetitorCategoryService.EditCompetitorCategoryAsync(cat);
            }

            cat = result;
            Id = result.Id;

            NavigationManager.NavigateTo($"/Masterdata/CompetitorCategoryDetails/{Id}");
        }
    }
}