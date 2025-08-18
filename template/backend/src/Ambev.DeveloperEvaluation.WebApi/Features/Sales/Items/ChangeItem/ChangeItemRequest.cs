namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.ChangeItem;

public record ChangeItemRequest(Guid SaleId, Guid ItemId, int Quantity, decimal UnitPrice);
