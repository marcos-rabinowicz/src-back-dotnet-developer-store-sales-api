namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public readonly record struct BranchIdentity(Guid Id, string Name)
{
    public static BranchIdentity Create(Guid id, string name)
    {
        if (id == Guid.Empty) throw new ArgumentException("Branch id cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Branch name is required.", nameof(name));
        return new BranchIdentity(id, name.Trim());
    }
}
