using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record class BranchIdentity
{
    public Guid Id { get; }
    public string? Name { get; }

    private BranchIdentity() { } // EF
    private BranchIdentity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static BranchIdentity Create(Guid id, string name)
    {
        if (id == Guid.Empty)
            throw new DomainException("Branch id cannot be empty.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Branch name cannot be empty.");
        return new BranchIdentity(id, name.Trim());
    }
}
