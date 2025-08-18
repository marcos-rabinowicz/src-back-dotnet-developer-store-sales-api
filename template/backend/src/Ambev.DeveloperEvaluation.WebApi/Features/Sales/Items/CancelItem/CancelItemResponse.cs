namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelItem;

public record CancelItemResponse(Guid SaleId, Guid ItemId, decimal SaleTotalAmount);
