namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record GetSaleItemResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal LineTotal
);

public record GetSaleResponse(
    Guid Id,
    string SaleNumber,
    DateTime Date,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    decimal TotalAmount,
    string Status,
    IReadOnlyCollection<GetSaleItemResponse> Items
);
