using System;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Unit.Domain.TestData;

internal static class IdentitiesFactory
{
    public static CustomerIdentity AnyCustomer()
        => CustomerIdentity.Create(Guid.NewGuid(), "Customer A");

    public static BranchIdentity AnyBranch()
        => BranchIdentity.Create(Guid.NewGuid(), "Branch RJ");

    public static ProductIdentity Product(string name = "Beer Pilsen")
        => ProductIdentity.Create(Guid.NewGuid(), name);
}
