# Blank7 Solution Template for .NET 7 Applications

Blank7 is a template solution for .NET 7 applications, providing a structure for organizing various components of a .NET application, including frontend, backend, and common/shared elements. The solution includes several projects targeting different frontend technologies like MAUI, Blazor WebAssembly, and Blazor Server, along with backend and common projects for handling domain models, API models, server, and persistence.

## Getting Started

To get started with the Blank7 solution, follow these steps:

1. Clone or download the repository.
2. Open the solution in Visual Studio or another compatible IDE.
3. Set up the database and Entity Framework (EF) according to the instructions below.

### Database Setup and EF Initialization

#### SQLite

The SQLite database is the default configuration in the `appsettings.json` file of the `BackEnd.Server` project. To initialize the SQLite database and EF, follow these steps:

1. Ensure the `Provider` field in the `DatabaseConfig` section of the `appsettings.json` file is set to "Sqlite":

   ```
   "DatabaseConfig": {
     "Provider": "Sqlite", // or "PostgreSQL", "MSSQL", "MySQL", "Oracle"
     ...
   },
   ```

2. Ensure the `SqliteConnectionString` in the `DatabaseConfig` section of the `appsettings.json` file is set:

   ```
   "DatabaseConfig": {
     ...
     "SqliteConnectionString": "Data Source=blank7.db",
     ...
   },
   ```

#### Other Database Systems

For other database systems (PostgreSQL, MSSQL, MySQL, or Oracle), follow these general steps:

1. Update the `Provider` field in the `DatabaseConfig` section of the `appsettings.json` file to the desired provider:

   ```
   "DatabaseConfig": {
     "Provider": "PostgreSQL", // or "MSSQL", "MySQL", "Oracle"
     ...
   },
   ```

2. Set the corresponding connection string for the chosen provider in the `DatabaseConfig` section.

### Handling Migrations

#### Adding a New Migration

To add a new migration, follow these steps:

1. Open a terminal or command prompt and navigate to the `BackEnd.Server` project folder.
2. Run the following command, replacing `MigrationName` with a descriptive name for your migration:

   ```
   dotnet ef migrations add MigrationName
   ```

#### Removing a Migration

To remove a migration, follow these steps:

1. Open a terminal or command prompt and navigate to the `BackEnd.Server` project folder.
2. Run the following command:

   ```
   dotnet ef migrations remove
   ```

### Updating the Database

After adding or removing migrations, update the database with the following command:

1. Open a terminal or command prompt and navigate to the `BackEnd.Server` project folder.
2. Run the following command:

   ```
   dotnet ef database update
   ```
