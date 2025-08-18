namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.ChangeItem;

public record ChangeItemResponse(Guid SaleId, Guid ItemId, decimal SaleTotalAmount);
