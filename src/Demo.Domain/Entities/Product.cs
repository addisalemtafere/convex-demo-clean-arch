using Demo.Contracts.Enums;
using Demo.Domain.Entities.Base;

namespace Demo.Domain.Entities;

public class Product : AuditableEntity
{
    public long Id { get; set; }
    public Guid PublicId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Active;
}
