namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.AddItem;

public record AddItemResponse(
    Guid SaleId,
    Guid ItemId,
    int Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal LineTotal,
    decimal SaleTotalAmount
);
