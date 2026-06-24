using Demo.Application.Common;

namespace Demo.Application.Features.ProductManagement.ListProducts;

public sealed record ListProductsQuery : IQuery<IReadOnlyList<ProductListItemDto>>;

public sealed record ProductListItemDto(
    Guid PublicId,
    string Name,
    decimal Price,
    string Status);
