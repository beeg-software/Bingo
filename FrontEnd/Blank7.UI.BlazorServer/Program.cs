using Blank7.UI.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IUserService, UserService>();

// Load the ApiConfiguration section and retrieve the BaseAddress value
var apiConfigurationSection = builder.Configuration.GetSection("ApiConfiguration");
string baseAddress = apiConfigurationSection.GetValue<string>("BaseAddress");

// Register HttpClient as a scoped service
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();