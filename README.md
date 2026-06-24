# convex-demo-clean-arch

Beginner-friendly **Clean Architecture + CQRS + EF Core + PostgreSQL** demo.  
Folder layout follows the same style as **Convex Identity**.

---

## What you get

- 5 layers under `src/` (Contracts, Domain, Application, Infrastructure, Presentation)
- **CQRS** with MediatR (commands + queries)
- **FluentValidation** pipeline
- **EF Core** with PostgreSQL
- **Swagger** UI on startup

---

## Prerequisites

Install these before you start:

| Tool | Version | Check |
|------|---------|-------|
| [.NET SDK](https://dotnet.microsoft.com/download) | **10.x** | `dotnet --version` |
| [PostgreSQL](https://www.postgresql.org/download/) | **14+** | `psql --version` |
| [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) | latest | `dotnet ef --version` |

Install EF tools if missing:

```bash
dotnet tool install --global dotnet-ef
```

---

## Step 1 — Clone the repository

```bash
git clone https://github.com/addisalemtafere/convex-demo-clean-arch.git
cd convex-demo-clean-arch
```

Checkout the CQRS branch (if not on `main`):

```bash
git checkout feature/cqrs-clean-architecture
```

---

## Step 2 — Restore and build

From the repo root:

```bash
dotnet restore demo.slnx
dotnet build demo.slnx
```

You should see **Build succeeded** with 0 errors.

---

## Step 3 — Start PostgreSQL

Make sure PostgreSQL is running on your machine.

**macOS (Homebrew example):**

```bash
brew services start postgresql@16
```

**Check it is running:**

```bash
pg_isready -h localhost -p 5432
```

---

## Step 4 — Configure the connection string

Open:

```
src/Demo.Presentation/appsettings.json
```

Default connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=demo;Username=postgres;Password=112233"
  }
}
```

**Update for your machine:**

| Setting | Description |
|---------|-------------|
| `Host` | Usually `localhost` |
| `Port` | Usually `5432` |
| `Database` | Database name (`demo`) |
| `Username` | Postgres user (often `postgres`) |
| `Password` | Your local Postgres password |

**Tip:** For local-only secrets, override in `appsettings.Development.json` instead of committing passwords.

---

## Step 5 — Create the database

Create the `demo` database (only needed once):

```bash
createdb -h localhost -p 5432 -U postgres demo
```

Or with `psql`:

```bash
psql -h localhost -p 5432 -U postgres -c "CREATE DATABASE demo;"
```

---

## Step 6 — Add EF Core migrations (you do this)

Migrations live in:

```
src/Demo.Infrastructure/Persistence/Migrations/
```

If the folder is empty (or you changed the model), **add a migration yourself**:

```bash
dotnet ef migrations add InitialCreate \
  --project src/Demo.Infrastructure/Demo.Infrastructure.csproj \
  --startup-project src/Demo.Presentation/Demo.Presentation.csproj \
  --output-dir Persistence/Migrations
```

When you change entities later, add a new migration:

```bash
dotnet ef migrations add YourMigrationName \
  --project src/Demo.Infrastructure/Demo.Infrastructure.csproj \
  --startup-project src/Demo.Presentation/Demo.Presentation.csproj \
  --output-dir Persistence/Migrations
```

---

## Step 7 — Apply migrations to the database

```bash
dotnet ef database update \
  --project src/Demo.Infrastructure/Demo.Infrastructure.csproj \
  --startup-project src/Demo.Presentation/Demo.Presentation.csproj
```

This creates tables in PostgreSQL (e.g. `Products`, `__EFMigrationsHistory`).

**Note:** The app also runs `MigrateAsync()` on startup, so pending migrations are applied automatically when you run the API.

---

## Step 8 — Run the API

```bash
dotnet run --project src/Demo.Presentation/Demo.Presentation.csproj
```

Or with the HTTP profile:

```bash
dotnet run --project src/Demo.Presentation/Demo.Presentation.csproj --launch-profile http
```

Swagger opens automatically at:

**http://localhost:5277/swagger**

---

## Step 9 — Test the API

### Option A — Swagger UI

1. Open **http://localhost:5277/swagger**
2. Try **POST /api/products**
3. Try **GET /api/products**
4. Copy `publicId` from the create response and try **GET /api/products/{publicId}**

### Option B — curl

**Create a product:**

```bash
curl -X POST http://localhost:5277/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Demo Widget",
    "description": "My first product",
    "price": 29.99,
    "status": 1
  }'
```

**List products:**

```bash
curl http://localhost:5277/api/products
```

**Get one product** (replace `{publicId}`):

```bash
curl http://localhost:5277/api/products/{publicId}
```

### Option C — `.http` file

Use `src/Demo.Presentation/Demo.Presentation.http` in Rider or VS Code REST Client.

### ProductStatus values

| Value | Name |
|-------|------|
| `0` | Draft |
| `1` | Active |
| `2` | Discontinued |

---

## Step 10 — Understand the request flow (CQRS)

Example: **Create Product**

```
HTTP POST /api/products
    ↓
ProductEndpoints (Presentation)
    ↓
MediatR.Send(CreateProductCommand)
    ↓
ValidationBehavior (FluentValidation)
    ↓
CreateProductCommandHandler (Application)
    ↓
IProductRepository + IUnitOfWork (Domain interfaces)
    ↓
ProductRepository + AppDbContext (Infrastructure)
    ↓
PostgreSQL
```

---

## Project structure

```
convex-demo-clean-arch/
├── demo.slnx
├── Directory.Build.props          # shared build settings
├── Directory.Packages.props       # central NuGet versions
├── README.md
└── src/
    ├── Demo.Contracts/            # enums, shared types
    ├── Demo.Domain/               # entities, repo interfaces
    ├── Demo.Application/          # CQRS handlers, validators
    ├── Demo.Infrastructure/       # EF Core, repos, migrations
    └── Demo.Presentation/         # API, Swagger, Program.cs
```

### Layer dependencies

```
Contracts
    ↑
Domain
    ↑
Application
    ↑
Infrastructure
    ↑
Presentation
```

### Feature folder (Identity style)

Each use case gets its own folder:

```
src/Demo.Application/Features/ProductManagement/
├── CreateProduct/
│   ├── CreateProductCommand.cs
│   ├── CreateProductCommandHandler.cs
│   ├── CreateProductCommandValidator.cs
│   └── CreateProductDto.cs
├── GetProduct/
│   ├── GetProductQuery.cs
│   ├── GetProductQueryHandler.cs
│   └── GetProductDto.cs
└── ListProducts/
    ├── ListProductsQuery.cs
    └── ListProductsQueryHandler.cs
```

---

## Step 11 — Add a new feature (optional)

Follow this order:

1. **Domain** — add entity + `IYourRepository` in `Demo.Domain`
2. **Infrastructure** — add EF config, repository, register in `DependencyInjection.cs`
3. **Application** — add folder under `Features/YourFeature/` with Command/Query, Handler, Validator, Dto
4. **Presentation** — add endpoint in `Endpoints/` and map it in `Program.cs`

Handlers are discovered automatically by MediatR.

---

## Common commands (cheat sheet)

```bash
# Build
dotnet build demo.slnx

# Run
dotnet run --project src/Demo.Presentation/Demo.Presentation.csproj

# Add migration
dotnet ef migrations add MigrationName \
  --project src/Demo.Infrastructure/Demo.Infrastructure.csproj \
  --startup-project src/Demo.Presentation/Demo.Presentation.csproj \
  --output-dir Persistence/Migrations

# Update database
dotnet ef database update \
  --project src/Demo.Infrastructure/Demo.Infrastructure.csproj \
  --startup-project src/Demo.Presentation/Demo.Presentation.csproj

# Remove last migration (if not applied)
dotnet ef migrations remove \
  --project src/Demo.Infrastructure/Demo.Infrastructure.csproj \
  --startup-project src/Demo.Presentation/Demo.Presentation.csproj
```

---

## Troubleshooting

### `password authentication failed for user "postgres"`

- Fix `Password` in `appsettings.json` or `appsettings.Development.json`

### `Connection refused` on port 5432

- Start PostgreSQL: `brew services start postgresql@16`
- Confirm port: `pg_isready -h localhost -p 5432`

### `database "demo" does not exist`

```bash
createdb -h localhost -U postgres demo
```

### Port 5277 already in use

- Stop the other process, or change `applicationUrl` in  
  `src/Demo.Presentation/Properties/launchSettings.json`

### `dotnet ef` not found

```bash
dotnet tool install --global dotnet-ef
```

### Migration already exists

- Use a new name, or remove the last migration with `dotnet ef migrations remove`

---

## API reference

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/products` | Create a product |
| GET | `/api/products` | List all products |
| GET | `/api/products/{publicId}` | Get one product by public ID |

---

## Packages

All NuGet versions are managed in one place:

```
Directory.Packages.props
```

Projects reference packages **without** a version:

```xml
<PackageReference Include="MediatR"/>
```
