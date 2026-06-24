using Demo.Application.Common;
using Demo.Domain.Entities;
using Demo.Domain.Interfaces;
using Demo.Domain.Repositories;
using MediatR;

namespace Demo.Application.Features.ProductManagement.CreateProduct;

public sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateProductCommand, CreateProductDto>, IRequestHandler<CreateProductCommand, CreateProductDto>
{
    public async Task<CreateProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            PublicId = Guid.CreateVersion7(),
            Name = command.Name.Trim(),
            Description = command.Description?.Trim(),
            Price = command.Price,
            Status = command.Status,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var created = await productRepository.CreateAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateProductDto(
            created.Id,
            created.PublicId,
            created.Name,
            created.Description,
            created.Price,
            created.Status.ToString(),
            created.CreatedAt);
    }
}
