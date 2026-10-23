# SEPMS

SEPMS is a web-based **Staff and Employee Profile Management System**. It lets users manage departments and employees, including employee-to-department assignments, and find records through search, filters, sorting, and pagination.

## Technology

- .NET 10 and ASP.NET Core MVC
- Entity Framework Core 10
- SQL Server / SQL Server LocalDB
- Razor views, Bootstrap, jQuery validation

## Features

- Create, view, edit, and delete departments
- Create, view, edit, and delete employees
- Assign each employee to a department
- Filter employee records by department and active status
- Filter department records by active status
- Search, sort, and paginate both lists
- Server-side and client-side form validation
- Responsive organization-themed UI for department and employee pages

## Solution structure

```text
SEPMS/                             ASP.NET Core MVC web project
+-- Controllers/                    HTTP endpoints and view orchestration
+-- Views/                          Razor UI templates
+-- Models/                         Web-specific view models
+-- SEPMS.Domain/                   Core entities and validation attributes
|   `-- Entities/                   Department and Employee
+-- SEPMS.Application/              Business services and abstractions
|   +-- Abstractions/               Service and repository contracts
|   +-- Common/                     Search, sort, and pagination models
|   `-- Services/                   Department and employee business logic
+-- SEPMS.Infrastructure/           EF Core and persistence implementation
|   `-- Persistence/                DbContext, repositories, migrations
+-- wwwroot/                        CSS, JavaScript, and client libraries
`-- Program.cs                      Dependency injection and HTTP pipeline
```

The dependency direction is:

```text
MVC controllers -> Application services -> Repository interfaces -> EF Core repositories
                                  ^                     ^
                                  |                     |
                                  +--- Domain entities --+
```

## Data model

### Department

| Field | Description |
| --- | --- |
| `DepartmentId` | Primary key |
| `DepartmentName` | Required department name, up to 100 characters |
| `DepartmentCode` | Required department code, up to 20 characters |
| `Description` | Optional description, up to 250 characters |
| `IsActive` | Active/inactive status |
| `CreatedDate` | Date the record was created |

### Employee

| Field | Description |
| --- | --- |
| `EmployeeId` | Primary key |
| `Name` | Required name, up to 100 characters |
| `Email` | Required valid email, up to 150 characters |
| `Phone` | Optional phone number, up to 20 characters |
| `JobTitle` | Required job title, up to 100 characters |
| `DateOfJoining` | Joining date |
| `IsActive` | Active/inactive status |
| `DepartmentId` | Required foreign key to `Department` |

One department can contain many employees. The current database migration configures cascading deletion: deleting a department also deletes its employees.

## Requirements

- .NET 10 SDK
- SQL Server LocalDB (default configuration), or an accessible SQL Server instance
- EF Core CLI, if it is not already installed:

```powershell
dotnet tool install --global dotnet-ef
```

## Getting started

1. Clone the repository and move to the project directory.
2. Restore packages:

```powershell
dotnet restore SEPMS.slnx
```

3. Configure the connection string. The default is in `appsettings.json`:

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SEPMS_DB;Trusted_Connection=True;TrustServerCertificate=True;"
```

4. Create or update the database using the Infrastructure migrations:

```powershell
dotnet ef database update --project SEPMS.Infrastructure\SEPMS.Infrastructure.csproj --startup-project SEPMS.csproj
```

5. Run the application:

```powershell
dotnet run --project SEPMS.csproj
```

In the default development profile, browse to `https://localhost:7161` or `http://localhost:5113`.

## Common commands

```powershell
# Build the whole solution
dotnet build SEPMS.slnx

# Add a migration after changing the entity model
dotnet ef migrations add <MigrationName> --project SEPMS.Infrastructure\SEPMS.Infrastructure.csproj --startup-project SEPMS.csproj

# Apply pending migrations
dotnet ef database update --project SEPMS.Infrastructure\SEPMS.Infrastructure.csproj --startup-project SEPMS.csproj
```

## Configuration

`Program.cs` registers MVC, application services, infrastructure repositories, SQL Server EF Core, and persistent ASP.NET Core data-protection keys.

The connection string is resolved from `ConnectionStrings:DefaultConnection`. For deployment, provide it through secure environment-specific configuration rather than committing production credentials.

## Current limitations

- Authentication and role-based authorization are not implemented.
- There are no automated tests yet.
- Department codes and employee emails are validated for format but are not currently enforced as unique in the database.
- The application does not automatically apply migrations at startup; use the migration command during setup/deployment.

## License

See [LICENSE](LICENSE).
