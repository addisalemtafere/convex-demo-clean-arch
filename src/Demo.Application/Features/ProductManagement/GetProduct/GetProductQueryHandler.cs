using Demo.Application.Common;
using Demo.Domain.Repositories;
using MediatR;

namespace Demo.Application.Features.ProductManagement.GetProduct;

public sealed class GetProductQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductQuery, GetProductDto?>, IRequestHandler<GetProductQuery, GetProductDto?>
{
    public async Task<GetProductDto?> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByPublicIdAsync(query.PublicId, cancellationToken);
        if (product is null)
        {
            return null;
        }

        return new GetProductDto(
            product.Id,
            product.PublicId,
            product.Name,
            product.Description,
            product.Price,
            product.Status.ToString(),
            product.CreatedAt);
    }
}
