using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

internal static class CreateSaleCommandFactory
{
    public static CreateSaleCommand Default() =>
        new(
            SaleNumber: "S-0001",
            Date: DateTime.UtcNow,
            CustomerId: Guid.NewGuid(),
            CustomerName: "Customer A",
            BranchId: Guid.NewGuid(),
            BranchName: "Branch RJ",
            Items: new List<CreateSaleItemDto>
            {
                new(Guid.NewGuid(), "Beer A", 3, 10m),  // 30
                new(Guid.NewGuid(), "Beer B", 4, 10m),  // 36 (10%)
                new(Guid.NewGuid(), "Beer C", 10, 10m), // 80 (20%)
            }
        );
}
