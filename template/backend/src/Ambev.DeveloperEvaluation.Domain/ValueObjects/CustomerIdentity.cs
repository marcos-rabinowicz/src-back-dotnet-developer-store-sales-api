using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record class CustomerIdentity
{
    public Guid Id { get; }
    public string? Name { get; }

    private CustomerIdentity() { } 
    private CustomerIdentity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static CustomerIdentity Create(Guid id, string name)
    {
        if (id == Guid.Empty)
            throw new DomainException("Customer id cannot be empty.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Customer name cannot be empty.");
        return new CustomerIdentity(id, name.Trim());
    }
}
