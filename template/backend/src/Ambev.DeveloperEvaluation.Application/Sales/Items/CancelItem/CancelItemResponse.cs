namespace Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

public record CancelItemResponse(Guid SaleId, Guid ItemId, decimal SaleTotalAmount);
