using Demo.Domain.Repositories;
using FluentValidation;

namespace Demo.Application.Features.ProductManagement.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IProductRepository productRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (name, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return true;
                }

                return !await productRepository.NameExistsAsync(name.Trim(), cancellation);
            })
            .WithMessage("A product with this name already exists.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
