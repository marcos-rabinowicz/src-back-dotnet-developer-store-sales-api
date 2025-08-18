namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public record GetSaleItemResult(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal LineTotal
);

public record GetSaleResult(
    Guid Id,
    string SaleNumber,
    DateTime Date,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    decimal TotalAmount,
    string Status,
    IReadOnlyCollection<GetSaleItemResult> Items
);
