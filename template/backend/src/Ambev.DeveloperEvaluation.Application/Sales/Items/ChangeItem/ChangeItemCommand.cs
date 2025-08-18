using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.ChangeItem;

public record ChangeItemCommand(
    Guid SaleId,
    Guid ItemId,
    int Quantity,
    decimal UnitPrice
) : IRequest<ChangeItemResponse>;
