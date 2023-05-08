// Import required namespaces
using Bingo.Common.DomainModel.MasterData;
using Bingo.UI.Shared.Services;
using Microsoft.AspNetCore.Components;

// Define the UserDetails component within the Bingo.UI.Shared.Components.MasterData namespace
namespace Bingo.UI.Shared.Components.MasterData
{
    public partial class UserDetails
    {
        // Inject required services for dependency injection
        [Inject] public IUserService UserService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        // Define the Id parameter to store the user's unique identifier
        [Parameter] public Guid Id { get; set; }

        // Define a private User object to store user details
        private User us { get; set; }

        // Override the OnInitializedAsync lifecycle method to initialize the component
        protected override async Task OnInitializedAsync()
        {
            // Initialize a new User object if the Id is empty, otherwise fetch the user details by Id
            if (Id == Guid.Empty)
            {
                us = new User(Guid.Empty, "", DateTime.MinValue);
            }
            else
            {
                us = await UserService.GetUserByIdAsync(Id);
            }

            // Call the base OnInitialized method
            base.OnInitialized();
        }

        // Define the HandleValidSubmit method to handle form submissions
        private async Task HandleValidSubmit()
        {
            // Initialize a new User object to store the result
            var result = new User(Guid.Empty, "", DateTime.MinValue);

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