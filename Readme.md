# Backend Documentation





##  Tech Stack

- ASP.NET Core (.NET 10)
- Entity Framework Core
- SQL Server (or any EF provider you configure)
- xUnit (for testing)

---

##  Requirements

Before running the project, make sure you have:

- .NET 10 SDK  
- SQL Server (LocalDB, full SQL Server, or Docker — anything supported by EF Core)

---

##  Configure the Database

Open the configuration file:

HMCT/appsettings.json

csharp
Copy code
Add your connection string under:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MyDb;Trusted_Connection=True;"
  }
}
Replace the connection string with your own SQL Server instance.

 Restore Packages & Run Migrations
Run these commands inside the backend folder:

powershell
Copy code
cd HMCT

# Restore NuGet packages
dotnet restore

# Create a migration (only when your models change)
dotnet ef migrations add InitialCreate

# Apply migrations to the database
dotnet ef database update

# Start the API
dotnet run
 API Base URL
Once running, the backend is available at:

arduino
Copy code
http://localhost:5160
 Example Endpoint
Create a new task:

bash
Copy code
POST /api/TaskItem/createTask
Request body:

json
Copy code
{
  "title": "My Task",
  "description": "Task details",
  "taskstatus": "Pending",
  "dueDateTime": "2025-01-10T10:00:00Z"
}
 Running Tests
Unit tests are located in the test project:

Copy code
HMCT.Tests
Run them with:

powershell
Copy code
cd HMCT.Tests
dotnet test
One test intentionally fails — this is expected.

 Project Structure
bash
Copy code
HMCT/                       # Backend API
  Controllers/              # API Controllers
  Models/                   # Entity models
  AppDbContext.cs           # EF Core Context
  appsettings.json          # Configuration

HMCT.Tests/                 # Unit tests



HMCT Task backend API built with:
1. ASP.NET Core (.NET 10)  
2. Entity Framework Core  
3. Code-First Migrations  
# My App — Backend Documentation



---

## Tech stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (configurable) or EF providers
- xUnit for tests (EF In-Memory for unit tests)

---

## Requirements

- .NET 10 SDK
- SQL Server (LocalDB, full SQL Server, or a containerized instance)

---

## Quick configuration

Edit the backend configuration file `HMCT/appsettings.json` and set your connection string under `ConnectionStrings:DefaultConnection`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MyDb;Trusted_Connection=True;"
  }
}
```

Replace the connection string with your SQL Server instance details.

---

## Common commands (powershell)

Run these from the repository root or the `HMCT` folder where the backend project lives.

```powershell
# Change to backend folder
cd HMCT

# Restore NuGet packages
dotnet restore

# Add a migration (do this when your models change)
dotnet ef migrations add InitialCreate

# Apply migrations to the database
dotnet ef database update

# Run the API
dotnet run
```

The API typically listens at `http://localhost:5160` by default.

---

## Project structure (backend)

```
HMCT/                   # Backend API project
  Controllers/          # API controllers (e.g. TaskItemController.cs)
  Models/               # Entity models (e.g. TaskItem.cs)
  AppDbContext.cs       # EF Core DbContext
  appsettings.json      # Configuration
  Program.cs            # App entry

HMCT.Tests/             # Unit tests (xUnit)
```

---

## Model example — TaskItem

Represents a task stored in the database. Keep properties simple and validated appropriately.

```csharp
public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string Taskstatus { get; set; } = "NotStarted";
    public required DateTime DueDateTime { get; set; }
}
```

Field notes:
- `Title`: required, consider MaxLength(25)
- `Description`: optional, consider MaxLength(150)

---

## DbContext example — AppDbContext

Configure entities and constraints in `OnModelCreating`.

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<TaskItem> TasksItem { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(25);
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.DueDateTime).IsRequired();
        });
    }
}
```

---

## Controller example — TaskItemController

Keep controllers small: validate request, map to entity, persist, return appropriate status codes.

```csharp
[ApiController]
[Route("api/[controller]")]
public class TaskItemController : ControllerBase
{
    private readonly AppDbContext _db;
    public TaskItemController(AppDbContext db) => _db = db;

    [HttpPost("createTask")]
    public async Task<IActionResult> CreateTask(CreateTaskRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Taskstatus = request.Taskstatus ?? "NotStarted",
            DueDateTime = request.DueDateTime
        };

        _db.TasksItem.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(null, new { id = entity.Id }, entity);
    }
}
```

Replace `CreateTaskRequest` with your DTO and adjust validation as needed.

---

## API example

Endpoint to create a task:

```
POST /api/TaskItem/createTask
Content-Type: application/json
```

Example request body:

```json
{
  "title": "Clean Kitchen",
  "description": "Remove trash",
  "taskstatus": "Pending",
  "dueDateTime": "2025-01-10T10:00:00Z"
}
```

Responses:
- ` Created` — task created (returns message body with task details)
- `400 Bad Request` — validation failed

---

## Tests

Run unit tests from the repo root or the `HMCT.Tests` folder:

```powershell
cd HMCT.Tests
dotnet test
```

Use EF In-Memory for fast unit tests; reserve real DB integration tests for CI or explicit runs.





