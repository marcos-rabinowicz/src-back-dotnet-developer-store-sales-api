namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelItem;

public record CancelItemRequest(Guid SaleId, Guid ItemId);
