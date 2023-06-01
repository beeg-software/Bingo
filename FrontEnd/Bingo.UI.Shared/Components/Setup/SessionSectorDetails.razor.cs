using Bingo.Common.DomainModel.Setup;
using Bingo.UI.Shared.Services.Setup;
using Microsoft.AspNetCore.Components;
using System.Runtime.Intrinsics.X86;

namespace Bingo.UI.Shared.Components.Setup
{
    public partial class SessionSectorDetails
    {
        [Inject] public ISessionSectorService SessionSectorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid Id { get; set; }

        // Properties for component's internal state.
        private SessionSector sese { get; set; }

        // Component initialization and sessionSector data fetch.
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                sese = new SessionSector();
                sese.Id = Guid.Empty;
                sese.SessionId = Guid.Empty;
                sese.SectorId = Guid.Empty;
                sese.RaceEnabled = false;
                sese.CreationTimeStamp = DateTime.MinValue;
                sese.LastUpdateTimeStamp = DateTime.MinValue;
            }
            else
            {
                sese = await SessionSectorService.GetSessionSectorByIdAsync(Id);
            }

            base.OnInitialized();
        }

        // Handle form submissions
        private async Task HandleValidSubmit()
        {
            var result = new SessionSector();

            // Check if the Id is empty, then create a new sessionSector, otherwise edit the existing sessionSector
            if (Id == Guid.Empty)
            {
                result = await SessionSectorService.NewSessionSectorAsync(sese);
            }
            else
            {
                result = await SessionSectorService.EditSessionSectorAsync(sese);
            }

            // Update the SessionSector object and Id with the result
            sese = result;
            Id = result.Id;

            // Update the URL with the new sessionSector ID to reflect the changes
            NavigationManager.NavigateTo($"/Setup/SessionSectorDetails/{Id}");
        }
    }
}