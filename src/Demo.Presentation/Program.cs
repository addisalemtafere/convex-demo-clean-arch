using Demo.Application;
using Demo.Domain;
using Demo.Infrastructure;
using Demo.Presentation.Endpoints;
using Demo.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices();

var app = builder.Build();

await app.Services.MigrateDatabaseAsync();

app.UsePresentationPipeline();
app.MapProductEndpoints();

app.Run();
