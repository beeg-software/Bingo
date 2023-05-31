using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Bingo.UI.Shared.Services.MasterData;
using Bingo.UI.Shared.Services.Setup;
using Bingo.UI.Shared.Services.Timing;

namespace Bingo.UI.MAUI
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

            return builder.Build();
        }
    }
}