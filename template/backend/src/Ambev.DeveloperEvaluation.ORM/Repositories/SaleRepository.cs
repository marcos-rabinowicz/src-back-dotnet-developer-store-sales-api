using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _ctx;

    public SaleRepository(DefaultContext ctx) => _ctx = ctx;

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _ctx.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken ct = default)
    {
        await _ctx.Sales.AddAsync(sale, ct);
        await _ctx.SaveChangesAsync(ct);

        return sale;
    }

    public async Task UpdateAsync(Sale sale, CancellationToken ct = default)
    {
        var entry = _ctx.Entry(sale);

        if (entry.State == EntityState.Detached)
            entry.State = EntityState.Unchanged;

        await _ctx.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _ctx.Sales.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (entity is null) return;

        _ctx.Sales.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
    }
}
