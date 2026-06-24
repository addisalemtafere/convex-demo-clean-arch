using Demo.Application.Common;
using Demo.Domain.Repositories;
using MediatR;

namespace Demo.Application.Features.ProductManagement.ListProducts;

public sealed class ListProductsQueryHandler(IProductRepository productRepository)
    : IQueryHandler<ListProductsQuery, IReadOnlyList<ProductListItemDto>>,
        IRequestHandler<ListProductsQuery, IReadOnlyList<ProductListItemDto>>
{
    public async Task<IReadOnlyList<ProductListItemDto>> Handle(
        ListProductsQuery query,
        CancellationToken cancellationToken)
    {
        var products = await productRepository.ListAsync(cancellationToken);

        return products
            .Select(product => new ProductListItemDto(
                product.PublicId,
                product.Name,
                product.Price,
                product.Status.ToString()))
            .ToList();
    }
}
