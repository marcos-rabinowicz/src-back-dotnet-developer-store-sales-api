namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public readonly record struct ProductIdentity(Guid Id, string Name)
{
    public static ProductIdentity Create(Guid id, string name)
    {
        if (id == Guid.Empty) throw new ArgumentException("Product id cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name is required.", nameof(name));
        return new ProductIdentity(id, name.Trim());
    }
}
