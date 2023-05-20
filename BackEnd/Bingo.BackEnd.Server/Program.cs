using Bingo.BackEnd.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register controllers in the DI container
builder.Services.AddControllers();

// Configure CORS policy with allowed origins, methods, and headers
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Enable API explorer to provide metadata for Swagger/OpenAPI tools
builder.Services.AddEndpointsApiExplorer();

// Add Swagger generator to generate OpenAPI documentation
builder.Services.AddSwaggerGen();

// Configure the DbContext
var dbConfigurationSection = builder.Configuration.GetSection("DatabaseConfig");
var provider = dbConfigurationSection.GetValue<string>("Provider");

if (provider == "Sqlite")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(dbConfigurationSection.GetValue<string>("SqliteConnectionString")));
}
else if (provider == "PostgreSQL")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(dbConfigurationSection.GetValue<string>("PostgreSQLConnectionString")));
}
else if (provider == "MSSQL")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(dbConfigurationSection.GetValue<string>("MSSQLConnectionString")));
}
else if (provider == "MySQL")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(dbConfigurationSection.GetValue<string>("MySQLDBConnectionString"),
        ServerVersion.AutoDetect(dbConfigurationSection.GetValue<string>("MySQLDBConnectionString"))));
}
else if (provider == "Oracle")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseOracle(dbConfigurationSection.GetValue<string>("OracleConnectionString")));
}
else
{
    throw new InvalidOperationException("Invalid database provider specified.");
}

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enable Swagger and Swagger UI in development environment
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Use a custom error handler in non-development environments
    app.UseExceptionHandler("/Error");

    // Enable HSTS for secure connections in production
    // Learn more at https://aka.ms/aspnetcore-hsts
    app.UseHsts();
}

// Enforce HTTPS for all requests
app.UseHttpsRedirection();

// Apply the configured CORS policy to incoming requests
app.UseCors("CorsPolicy");

// Enable authorization middleware
app.UseAuthorization();

// Map controller endpoints to routes
app.MapControllers();

// Start the application
app.Run();