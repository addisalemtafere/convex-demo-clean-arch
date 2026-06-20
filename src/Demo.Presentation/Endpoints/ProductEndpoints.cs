using Demo.Application;
using Demo.Application.Features.ProductManagement.CreateProduct;
using Demo.Application.Features.ProductManagement.GetProduct;
using Demo.Application.Features.ProductManagement.ListProducts;
using Demo.Domain.Exceptions;
using Demo.Infrastructure;
using MediatR;

namespace Demo.Presentation.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        group.MapPost("/", async (CreateProductRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(
                    new CreateProductCommand
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Price = request.Price,
                        Status = request.Status
                    },
                    cancellationToken);

                return Results.Created($"/api/products/{result.PublicId}", result);
            })
            .WithName("CreateProduct");

        group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var products = await mediator.Send(new ListProductsQuery(), cancellationToken);
                return Results.Ok(products);
            })
            .WithName("ListProducts");

        group.MapGet("/{publicId:guid}", async (Guid publicId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var product = await mediator.Send(new GetProductQuery(publicId), cancellationToken);
                return product is null ? Results.NotFound() : Results.Ok(product);
            })
            .WithName("GetProduct");

        return app;
    }
}

public sealed record CreateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    Demo.Contracts.Enums.ProductStatus Status = Demo.Contracts.Enums.ProductStatus.Active);
