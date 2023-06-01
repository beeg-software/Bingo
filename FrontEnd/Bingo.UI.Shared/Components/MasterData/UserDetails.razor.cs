using Bingo.Common.DomainModel.MasterData;
using Bingo.UI.Shared.Services.MasterData;
using Microsoft.AspNetCore.Components;

namespace Bingo.UI.Shared.Components.MasterData
{
    public partial class UserDetails
    {
        [Inject] public IUserService UserService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid Id { get; set; }

        // Properties for component's internal state.
        private User us { get; set; }

        // Component initialization and user data fetch.
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                us = new User();
                us.Id = Guid.Empty;
                us.Name = string.Empty;
                us.CreationTimeStamp = DateTime.MinValue;
                us.LastUpdateTimeStamp = DateTime.MinValue;
            }
            else
            {
                us = await UserService.GetUserByIdAsync(Id);
            }

            base.OnInitialized();
        }

        // Handle form submissions
        private async Task HandleValidSubmit()
        {
            var result = new User();

            // Check if the Id is empty, then create a new user, otherwise edit the existing user
            if (Id == Guid.Empty)
            {
                result = await UserService.NewUserAsync(us);
            }
            else
            {
                result = await UserService.EditUserAsync(us);
            }

            // Update the User object and Id with the result
            us = result;
            Id = result.Id;

            // Update the URL with the new user ID to reflect the changes
            NavigationManager.NavigateTo($"/Masterdata/UserDetails/{Id}");
        }
    }
}