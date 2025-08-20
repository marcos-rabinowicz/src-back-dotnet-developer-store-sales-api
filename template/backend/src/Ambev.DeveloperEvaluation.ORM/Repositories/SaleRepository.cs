using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _ctx;

    public SaleRepository(DefaultContext ctx) => _ctx = ctx;

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _ctx.Sales
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _ctx.Sales.Add(sale);
        await _ctx.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _ctx.Sales.Update(sale);
        await _ctx.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _ctx.Sales.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (entity is null) return;

        _ctx.Sales.Remove(entity);
        await _ctx.SaveChangesAsync(cancellationToken);
    }
}
