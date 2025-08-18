namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public readonly record struct CustomerIdentity(Guid Id, string Name)
{
    public static CustomerIdentity Create(Guid id, string name)
    {
        if (id == Guid.Empty) throw new ArgumentException("Customer id cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Customer name is required.", nameof(name));
        return new CustomerIdentity(id, name.Trim());
    }
}
