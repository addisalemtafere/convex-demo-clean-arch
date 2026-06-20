# convex-demo-clean-arch

Clean architecture .NET solution with dependency-based project setup.

## Projects

| Project | References |
|---------|------------|
| Domain | — |
| Application | Domain |
| Infrastructure | Application, Domain |
| Presentation | Application, Infrastructure |

## Run

```bash
dotnet run --project Presentation/Presentation.csproj
```
