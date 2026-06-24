namespace Demo.Application.Features.ProductManagement.GetProduct;

public sealed record GetProductDto(
    long Id,
    Guid PublicId,
    string Name,
    string? Description,
    decimal Price,
    string Status,
    DateTimeOffset CreatedAt);
