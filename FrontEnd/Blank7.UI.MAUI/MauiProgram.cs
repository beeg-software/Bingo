using Microsoft.Extensions.Logging;
using Blank7.UI.Shared.Services;
using Microsoft.Extensions.Configuration;

namespace Blank7.UI.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Load the appsettings.json configuration file
            builder.Configuration.AddJsonFile("appsettings.json");

            // Configure the Maui app and fonts
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add Maui Blazor WebView support
            builder.Services.AddMauiBlazorWebView();

            // Add developer tools and debug logging for Debug builds
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

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

            return builder.Build();
        }
    }
}