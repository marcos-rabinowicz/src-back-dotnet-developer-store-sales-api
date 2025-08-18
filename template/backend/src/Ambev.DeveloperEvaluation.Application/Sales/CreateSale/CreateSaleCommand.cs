using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public record CreateSaleItemDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);

public record CreateSaleCommand(
    string SaleNumber,
    DateTime Date,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    IReadOnlyCollection<CreateSaleItemDto> Items
) : IRequest<CreateSaleResult>;
