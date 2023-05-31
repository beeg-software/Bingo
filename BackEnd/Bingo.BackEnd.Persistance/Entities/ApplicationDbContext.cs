using Bingo.Common.DomainModel.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Bingo.BackEnd.Persistance.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<CompetitorCategory> CompetitorCategories { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<SectorTime> SectorTimes { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionSector> SessionSectors { get; set; }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;

            // Split the assemblyDirectory by the "Bingo.BackEnd.Persistance" substring
            string[] pathParts = assemblyLocation.Split(new[] { @"\Bingo.BackEnd.Persistance" }, StringSplitOptions.None);
            string persistanceProjectPath = pathParts[0] + @"\Bingo.BackEnd.Persistance\";
            string serverProjectPath = pathParts[0] + @"\Bingo.BackEnd.Server\";


            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(serverProjectPath)
                .AddJsonFile(serverProjectPath + "appsettings.json")
                .Build();

            var dbConfigurationSection = configuration.GetSection("DatabaseConfig");
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            string provider = dbConfigurationSection.GetSection("Provider").Value;

            var connectionString = "";

            if (provider == "Sqlite")
            {
                connectionString = dbConfigurationSection.GetSection("SqliteConnectionString").Value;
                string[] connectionStringParts = connectionString.Split(new[] { @"Data Source=" }, StringSplitOptions.None);
                connectionString = @"Data Source=" + serverProjectPath + connectionStringParts[1];
                builder.UseSqlite(connectionString);
            }
            else if (provider == "PostgreSQL")
            {
                connectionString = dbConfigurationSection.GetSection("PostgreSQLConnectionString").Value;
                builder.UseNpgsql(connectionString);
            }
            else if (provider == "MSSQL")
            {
                connectionString = dbConfigurationSection.GetSection("MSSQLConnectionString").Value;
                builder.UseSqlServer(connectionString);
            }
            else if (provider == "MySQL")
            {
                connectionString = dbConfigurationSection.GetSection("MySQLDBConnectionString").Value;
                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
            else if (provider == "Oracle")
            {
                connectionString = dbConfigurationSection.GetSection("OracleConnectionString").Value;
                builder.UseOracle(connectionString);
            }
            else
            {
                throw new InvalidOperationException("Invalid database provider specified.");
            }

            return new ApplicationDbContext(builder.Options);
        }
    }
}