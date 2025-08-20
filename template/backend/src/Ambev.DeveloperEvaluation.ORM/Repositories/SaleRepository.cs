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
        // Para owned collection armazenada em campo privado "_items"
        return await _ctx.Sales
            .AsNoTracking()
            .Include("_items")
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task AddAsync(Sale sale, CancellationToken ct = default)
    {
        _ctx.Sales.Add(sale);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Sale sale, CancellationToken ct = default)
    {
        _ctx.Sales.Update(sale);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _ctx.Sales.FindAsync([id], ct);
        if (entity is null) return;
        _ctx.Sales.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
    }
}
