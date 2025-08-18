using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

public record CancelItemCommand(Guid SaleId, Guid ItemId) : IRequest<CancelItemResponse>;
