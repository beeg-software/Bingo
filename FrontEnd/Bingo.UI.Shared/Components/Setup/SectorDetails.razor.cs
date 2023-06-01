using Bingo.Common.DomainModel.Setup;
using Bingo.UI.Shared.Services.Setup;
using Microsoft.AspNetCore.Components;

namespace Bingo.UI.Shared.Components.Setup
{
    public partial class SectorDetails
    {
        [Inject] public ISectorService SectorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid Id { get; set; }

        // Properties for component's internal state.
        private Sector sc { get; set; }

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

            base.OnInitialized();
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