using Blank7.UI.BlazorWASM;
using Blank7.UI.Shared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// Create a WebAssemblyHostBuilder and configure root components
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Retrieve the API configuration and validate the base address
var apiConfigurationSection = builder.Configuration.GetSection("ApiConfiguration");
string baseAddress = apiConfigurationSection.GetValue<string>("BaseAddress");

if (string.IsNullOrEmpty(baseAddress))
{
    throw new InvalidOperationException("The API base address is not configured.");
}

// Register the HttpClient with the configured base address
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

// Register the IUserService implementation
builder.Services.AddScoped<IUserService, UserService>();

// Build and run the Blazor WebAssembly app
await builder.Build().RunAsync();