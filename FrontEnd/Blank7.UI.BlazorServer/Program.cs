using Blank7.UI.Shared.Services;

// Create a WebApplication builder
var builder = WebApplication.CreateBuilder(args);

// Configure services for the application
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IUserService, UserService>();

// Retrieve the API configuration and validate the base address
var apiConfigurationSection = builder.Configuration.GetSection("ApiConfiguration");
string baseAddress = apiConfigurationSection.GetValue<string>("BaseAddress");

// Check if the base address is configured, otherwise throw an exception
if (string.IsNullOrEmpty(baseAddress))
{
    throw new InvalidOperationException("The API base address is not configured.");
}

// Register the HttpClient with the configured base address
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

// Build the WebApplication
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure HTTPS redirection, static files, and routing
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Configure Blazor Hub and fallback routing
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Run the WebApplication
app.Run();