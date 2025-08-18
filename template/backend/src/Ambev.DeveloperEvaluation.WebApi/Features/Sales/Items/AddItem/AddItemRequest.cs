namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.AddItem;

public record AddItemRequest(
    Guid SaleId,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);
