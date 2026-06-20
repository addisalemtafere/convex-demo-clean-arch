using Demo.Application.Common;
using Demo.Contracts.Enums;

namespace Demo.Application.Features.ProductManagement.CreateProduct;

public record CreateProductCommand : ICommand<CreateProductDto>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public ProductStatus Status { get; init; } = ProductStatus.Active;
}
