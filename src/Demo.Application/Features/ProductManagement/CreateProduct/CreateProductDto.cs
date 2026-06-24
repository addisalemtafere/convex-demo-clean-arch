namespace Demo.Application.Features.ProductManagement.CreateProduct;

public sealed record CreateProductDto(
    long Id,
    Guid PublicId,
    string Name,
    string? Description,
    decimal Price,
    string Status,
    DateTimeOffset CreatedAt);
