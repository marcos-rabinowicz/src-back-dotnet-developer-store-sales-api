using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

public record AddItemCommand(
    Guid SaleId,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
) : IRequest<AddItemResult>;
