namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public record CreateSaleItemRequest(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);

public record CreateSaleRequest(
    string SaleNumber,
    DateTime Date,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    IReadOnlyCollection<CreateSaleItemRequest> Items
);
