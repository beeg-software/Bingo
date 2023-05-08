using Bingo.UI.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Bingo.Common.DomainModel.MasterData;

namespace Bingo.UI.Shared.Pages
{
    public partial class Index
    {
        #pragma warning disable CS8618
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        #pragma warning restore CS8618

        [Inject] protected IUserService UserService { get; set; }

        private List<User> _users = new List<User>();

        protected override async Task OnInitializedAsync()
        {
            _users = await UserService.GetUsersAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("Bingo.init");
                Console.WriteLine("From C#");
            }
        }
    }
}
