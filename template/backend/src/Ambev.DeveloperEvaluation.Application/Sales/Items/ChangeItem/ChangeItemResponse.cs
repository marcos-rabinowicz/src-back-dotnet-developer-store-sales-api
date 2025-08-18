namespace Ambev.DeveloperEvaluation.Application.Sales.Items.ChangeItem;

public record ChangeItemResponse(Guid SaleId, Guid ItemId, decimal SaleTotalAmount);
