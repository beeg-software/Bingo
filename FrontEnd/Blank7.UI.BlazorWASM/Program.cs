using Blank7.UI.BlazorWASM;
using Blank7.UI.Shared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load the ApiConfiguration section and retrieve the BaseAddress value
var apiConfigurationSection = builder.Configuration.GetSection("ApiConfiguration");
string baseAddress = apiConfigurationSection.GetValue<string>("BaseAddress");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

builder.Services.AddScoped<IUserService, UserService>();

await builder.Build().RunAsync();