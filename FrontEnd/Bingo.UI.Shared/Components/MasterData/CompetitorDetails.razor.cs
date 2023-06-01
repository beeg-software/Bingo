using Bingo.Common.DomainModel.MasterData;
using Bingo.UI.Shared.Services.MasterData;
using Microsoft.AspNetCore.Components;

namespace Bingo.UI.Shared.Components.MasterData
{
    public partial class CompetitorDetails
    {
        [Inject] public ICompetitorService CompetitorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid Id { get; set; }

        // Properties for component's internal state.
        private Competitor comp { get; set; }

        // Component initialization and competitor data fetch.
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                comp = new Competitor();
                comp.Id = Guid.Empty;
                comp.Name1 = string.Empty;
                comp.CreationTimeStamp = DateTime.MinValue;
                comp.LastUpdateTimeStamp = DateTime.MinValue;
            }
            else
            {
                comp = await CompetitorService.GetCompetitorByIdAsync(Id);
            }

            base.OnInitialized();
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