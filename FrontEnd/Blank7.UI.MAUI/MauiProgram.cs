using Microsoft.Extensions.Logging;
using Blank7.UI.Shared.Services;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Blank7.UI.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Load appsettings.json manually
        builder.Configuration.AddJsonFile("appsettings.json");

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var apiConfigurationSection = builder.Configuration.GetSection("ApiConfiguration");
        string baseAddress = apiConfigurationSection.GetValue<string>("BaseAddress");

        if (string.IsNullOrEmpty(baseAddress))
        {
            throw new InvalidOperationException("The API base address is not configured.");
        }

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

        builder.Services.AddScoped<IUserService, UserService>();

        return builder.Build();
    }
}