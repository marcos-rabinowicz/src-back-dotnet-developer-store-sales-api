using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

public sealed class AddItemHandler : IRequestHandler<AddItemCommand, AddItemResult>
{
    private readonly ISaleRepository _repo;

    public AddItemHandler(ISaleRepository repo)
    {
        _repo = repo;
    }

    public async Task<AddItemResult> Handle(AddItemCommand request, CancellationToken ct)
    {
        AddItemValidator.EnsureValid(request);

        var sale = await _repo.GetByIdAsync(request.SaleId, ct)
                   ?? throw new KeyNotFoundException($"Sale '{request.SaleId}' not found.");

        sale.AddItem(
            ProductIdentity.Create(request.ProductId, request.ProductName),
            request.Quantity,
            request.UnitPrice
        );

        var item = sale.Items.LastOrDefault()
                   ?? throw new InvalidOperationException("Item could not be added to the sale.");

        await _repo.UpdateAsync(sale, ct);

        return new AddItemResult(
            SaleId: sale.Id,
            ItemId: item.Id,
            Quantity: item.Quantity,
            UnitPrice: item.UnitPrice,
            DiscountPercent: item.DiscountPercent,
            LineTotal: item.LineTotal,
            SaleTotalAmount: sale.TotalAmount
        );
    }
}
