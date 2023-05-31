using Bingo.UI.BlazorWASM;
using Bingo.UI.Shared.Services.MasterData;
using Bingo.UI.Shared.Services.Setup;
using Bingo.UI.Shared.Services.Timing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// Create a WebAssemblyHostBuilder and configure root components
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Retrieve the API configuration and validate the base address
var apiConfigurationSection = builder.Configuration.GetSection("ApiConfiguration");
string baseAddress = apiConfigurationSection?.GetValue<string>("BaseAddress") ?? string.Empty;

if (string.IsNullOrWhiteSpace(baseAddress))
{
    throw new InvalidOperationException("The API base address is not configured.");
}

// Register the HttpClient with the configured base address
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

// Register the Services implementation
builder.Services.AddScoped<ICompetitorCategoryService, CompetitorCategoryService>();
builder.Services.AddScoped<ICompetitorService, CompetitorService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISectorService, SectorService>();
builder.Services.AddScoped<ISessionSectorService, SessionSectorService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ISectorTimeService, SectorTimeService>();



// Build and run the Blazor WebAssembly app
await builder.Build().RunAsync();