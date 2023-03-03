using Blank7.UI.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blank7.UI.Shared.Pages
{
    public partial class Index
    {
        #pragma warning disable CS8618
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        #pragma warning restore CS8618

        [Inject] protected IUserService UserService { get; set; }

        private List<string> _users = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            _users = await UserService.GetUsersAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("Blank7.init");
                Console.WriteLine("From C#");
            }
        }
    }
}
