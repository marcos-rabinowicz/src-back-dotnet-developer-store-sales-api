using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

public class CancelItemHandler : IRequestHandler<CancelItemCommand, CancelItemResponse>
{
    private readonly ISaleRepository _repo;

    public CancelItemHandler(ISaleRepository repo) => _repo = repo;

    public async Task<CancelItemResponse> Handle(CancelItemCommand request, CancellationToken ct)
    {
        CancelItemValidator.EnsureValid(request);

        var sale = await _repo.GetByIdAsync(request.SaleId, ct)
                   ?? throw new KeyNotFoundException("Sale not found.");

        sale.CancelItem(request.ItemId);
        await _repo.UpdateAsync(sale, ct);

        return new CancelItemResponse(sale.Id, request.ItemId, sale.TotalAmount);
    }
}
