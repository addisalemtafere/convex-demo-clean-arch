# Task Assignment — Task Management Feature

**For developers:** Implement a full **Task** feature using the existing **Product** feature as your reference.  
Do not modify Product code unless you find a shared bug.

---

## Goal

Add CRUD-style task management with Clean Architecture + CQRS + EF Core + PostgreSQL, matching the same patterns and folder layout as Product.

When finished, these endpoints must work:

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/tasks` | Create a task |
| GET | `/api/tasks` | List all tasks |
| GET | `/api/tasks/{publicId}` | Get one task by public ID |

Swagger must show a **Tasks** group with all three endpoints working.

---

## Reference (already implemented)

Study these files first — copy the pattern, rename types:

```
src/Demo.Application/Features/ProductManagement/
├── CreateProduct/
├── GetProduct/
└── ListProducts/

src/Demo.Domain/Entities/Product.cs
src/Demo.Domain/Repositories/IProductRepository.cs
src/Demo.Infrastructure/Persistence/Repositories/ProductRepository.cs
src/Demo.Infrastructure/Persistence/Configurations/ProductConfiguration.cs
src/Demo.Presentation/Endpoints/ProductEndpoints.cs
```

---

## Task domain model (you define)

Create a task entity with at least:

| Field | Type | Rules |
|-------|------|-------|
| `Id` | `long` | Database identity |
| `PublicId` | `Guid` | External API id (use `Guid.CreateVersion7()` on create) |
| `Title` | `string` | Required, max 200, **unique** |
| `Description` | `string?` | Optional, max 1000 |
| `DueDate` | `DateOnly?` | Optional; if set, must be today or future |
| `Status` | enum | `Todo`, `InProgress`, `Done` |
| `CreatedAt` | `DateTimeOffset` | Set on create |
| `UpdatedAt` | `DateTimeOffset?` | Optional |

Put the enum in `Demo.Contracts/Enums/`.  
Entity name suggestion: `TaskItem` (avoids clash with `System.Threading.Tasks.Task`).

---

## Step-by-step checklist

### 1. Demo.Contracts

- [ ] Add `TaskStatus` enum (`Todo = 0`, `InProgress = 1`, `Done = 2`)

### 2. Demo.Domain

- [ ] Add `Entities/TaskItem.cs` (extends `AuditableEntity` like Product)
- [ ] Add `Repositories/ITaskRepository.cs` with:
  - `GetByPublicIdAsync`
  - `TitleExistsAsync`
  - `CreateAsync`
  - `ListAsync`

### 3. Demo.Infrastructure

- [ ] Add `DbSet<TaskItem>` to `AppDbContext` and `IApplicationDbContext`
- [ ] Add `Persistence/Configurations/TaskItemConfiguration.cs`
- [ ] Add `Persistence/Repositories/TaskRepository.cs`
- [ ] Register `ITaskRepository` in `DependencyInjection.cs`
- [ ] Add EF migration:
  ```bash
  dotnet ef migrations add AddTasks \
    --project src/Demo.Infrastructure/Demo.Infrastructure.csproj \
    --startup-project src/Demo.Presentation/Demo.Presentation.csproj \
    --output-dir Persistence/Migrations
  ```
- [ ] Apply migration:
  ```bash
  dotnet ef database update \
    --project src/Demo.Infrastructure/Demo.Infrastructure.csproj \
    --startup-project src/Demo.Presentation/Demo.Presentation.csproj
  ```

### 4. Demo.Application

Create `Features/TaskManagement/` with three use cases:

#### CreateTask (copy from CreateProduct)

- [ ] `CreateTaskCommand.cs`
- [ ] `CreateTaskCommandHandler.cs`
- [ ] `CreateTaskCommandValidator.cs`
- [ ] `CreateTaskDto.cs`

Validation rules:

- Title required, max 200, unique (use `ITaskRepository.TitleExistsAsync`)
- Description max 1000 when provided
- DueDate must be today or later when provided
- Status must be valid enum value

#### GetTask (copy from GetProduct)

- [ ] `GetTaskQuery.cs`
- [ ] `GetTaskQueryHandler.cs`
- [ ] `GetTaskDto.cs`

Return `null` when not found → endpoint returns 404.

#### ListTasks (copy from ListProducts)

- [ ] `ListTasksQuery.cs`
- [ ] `ListTasksQueryHandler.cs`
- [ ] `TaskListItemDto` record (same style as `ProductListItemDto`)

Order results by title.

### 5. Demo.Presentation

- [ ] Add `Endpoints/TaskEndpoints.cs` (copy `ProductEndpoints.cs`)
- [ ] Map routes in `Program.cs`: `app.MapTaskEndpoints();`
- [ ] Add sample requests to `Demo.Presentation.http`

---

## Sample API requests

**Create task:**

```http
POST http://localhost:5277/api/tasks
Content-Type: application/json

{
  "title": "Learn CQRS",
  "description": "Complete the Task assignment",
  "dueDate": "2026-12-31",
  "status": 0
}
```

**List tasks:**

```http
GET http://localhost:5277/api/tasks
```

**Get task:**

```http
GET http://localhost:5277/api/tasks/{publicId}
```

---

## Verify your work

```bash
dotnet build demo.slnx
dotnet run --project src/Demo.Presentation/Demo.Presentation.csproj
```

1. Open http://localhost:5277/swagger  
2. Create a task → expect **201**  
3. List tasks → expect your task in the list  
4. Get by `publicId` → expect **200**  
5. Create duplicate title → expect **400** validation error  
6. Get unknown `publicId` → expect **404**

---

## Definition of done

- [ ] All layers follow Product folder naming and structure
- [ ] CQRS handlers registered via MediatR (no manual handler registration)
- [ ] FluentValidation runs through `ValidationBehavior`
- [ ] PostgreSQL migration committed
- [ ] Product endpoints still work unchanged
- [ ] No hardcoded secrets beyond existing `appsettings.json`

---

## Hints

- Implement **one layer at a time** and run `dotnet build` after each step
- If stuck, open Product and Task files side-by-side
- `TaskStatus` conflicts with `System.Threading.Tasks.TaskStatus` — use a type alias:
  ```csharp
  using TaskStatus = Demo.Contracts.Enums.TaskStatus;
  ```

Good luck!
