namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public record CreateSaleItemResult(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal LineTotal
);

public record CreateSaleResult(
    Guid Id,
    string SaleNumber,
    DateTime Date,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    decimal TotalAmount,
    string Status,
    IReadOnlyCollection<CreateSaleItemResult> Items
);
