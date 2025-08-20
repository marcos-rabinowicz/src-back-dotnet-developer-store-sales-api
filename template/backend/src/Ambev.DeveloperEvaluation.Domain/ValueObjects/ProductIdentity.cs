using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record class ProductIdentity
{
    public Guid Id { get; }
    public string? Name { get; }

    private ProductIdentity() { } // EF
    private ProductIdentity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static ProductIdentity Create(Guid id, string name)
    {
        if (id == Guid.Empty)
            throw new DomainException("Product id cannot be empty.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");
        return new ProductIdentity(id, name.Trim());
    }
}
