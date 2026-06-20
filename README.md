# convex-demo-clean-arch

Beginner-friendly **Clean Architecture + CQRS + EF Core** demo, structured like Convex Identity.

## Architecture

```
Presentation  →  Application  →  Domain
      ↓               ↑
Infrastructure ───────┘
Contracts (shared enums/types)
```

| Layer | Responsibility |
|-------|----------------|
| **Demo.Contracts** | Shared enums and cross-boundary types |
| **Demo.Domain** | Entities, repository interfaces, domain rules |
| **Demo.Application** | CQRS commands/queries, validators, MediatR handlers |
| **Demo.Infrastructure** | EF Core, repositories, Unit of Work |
| **Demo.Presentation** | HTTP API, Swagger, thin endpoint adapters |

## Folder style (Identity pattern)

```
src/
├── Demo.Contracts/Enums/
├── Demo.Domain/Entities/, Repositories/, Interfaces/
├── Demo.Application/
│   ├── Common/Behaviors/
│   └── Features/ProductManagement/
│       ├── CreateProduct/
│       ├── GetProduct/
│       └── ListProducts/
├── Demo.Infrastructure/Persistence/
│   ├── Data/
│   ├── Configurations/
│   └── Repositories/
└── Demo.Presentation/Endpoints/, Extensions/
```

## CQRS flow

1. **Command/Query** — `CreateProductCommand`, `GetProductQuery`
2. **Handler** — business logic in `*Handler.cs`
3. **Validator** — FluentValidation (runs via `ValidationBehavior`)
4. **Endpoint** — sends request through MediatR

## Run

```bash
dotnet run --project src/Demo.Presentation/Demo.Presentation.csproj
```

Swagger opens at **http://localhost:5277/swagger**.

## Sample API

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/products` | Create a product |
| GET | `/api/products` | List all products |
| GET | `/api/products/{publicId}` | Get one product |

## Packages

Versions are centralized in `Directory.Packages.props`.
