using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResponse>
{
    private readonly ISaleRepository _repo;

    public CancelSaleHandler(ISaleRepository repo) => _repo = repo;

    public async Task<CancelSaleResponse> Handle(CancelSaleCommand request, CancellationToken ct)
    {
        CancelSaleValidator.EnsureValid(request);

        var sale = await _repo.GetByIdAsync(request.Id, ct)
                   ?? throw new KeyNotFoundException("Sale not found.");

        sale.Cancel();
        await _repo.UpdateAsync(sale, ct);

        return new CancelSaleResponse(sale.Id, sale.Status.ToString());
    }
}
