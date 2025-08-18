namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

public record AddItemResult(
    Guid SaleId,
    Guid ItemId,
    int Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal LineTotal,
    decimal SaleTotalAmount
);
