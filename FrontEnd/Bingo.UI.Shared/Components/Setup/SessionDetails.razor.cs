using Bingo.Common.DomainModel.Setup;
using Bingo.UI.Shared.Services.Setup;
using Microsoft.AspNetCore.Components;

namespace Bingo.UI.Shared.Components.Setup
{
    public partial class SessionDetails
    {
        [Inject] public ISessionService SessionService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid Id { get; set; }

        // Properties for component's internal state.
        private Session ses { get; set; }

        // Component initialization and session data fetch.
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                ses = new Session();
                ses.Id = Guid.Empty;
                ses.Name = string.Empty;
                ses.CreationTimeStamp = DateTime.MinValue;
                ses.LastUpdateTimeStamp = DateTime.MinValue;
            }
            else
            {
                ses = await SessionService.GetSessionByIdAsync(Id);
            }

            base.OnInitialized();
        }

        // Handle form submissions
        private async Task HandleValidSubmit()
        {
            var result = new Session();

            // Check if the Id is empty, then create a new session, otherwise edit the existing session
            if (Id == Guid.Empty)
            {
                result = await SessionService.NewSessionAsync(ses);
            }
            else
            {
                result = await SessionService.EditSessionAsync(ses);
            }

            // Update the Session object and Id with the result
            ses = result;
            Id = result.Id;

            // Update the URL with the new session ID to reflect the changes
            NavigationManager.NavigateTo($"/Setup/SessionDetails/{Id}");
        }
    }
}