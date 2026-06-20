using Demo.Application.Common;

namespace Demo.Application.Features.ProductManagement.GetProduct;

public sealed record GetProductQuery(Guid PublicId) : IQuery<GetProductDto?>;
