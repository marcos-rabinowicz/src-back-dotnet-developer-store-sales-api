using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.ChangeItem;

public class ChangeItemHandler : IRequestHandler<ChangeItemCommand, ChangeItemResponse>
{
    private readonly ISaleRepository _repo;

    public ChangeItemHandler(ISaleRepository repo) => _repo = repo;

    public async Task<ChangeItemResponse> Handle(ChangeItemCommand request, CancellationToken ct)
    {
        ChangeItemValidator.EnsureValid(request);

        var sale = await _repo.GetByIdAsync(request.SaleId, ct)
                   ?? throw new KeyNotFoundException("Sale not found.");

        sale.ChangeItem(request.ItemId, request.Quantity, request.UnitPrice);
        await _repo.UpdateAsync(sale, ct);

        return new ChangeItemResponse(sale.Id, request.ItemId, sale.TotalAmount);
    }
}
