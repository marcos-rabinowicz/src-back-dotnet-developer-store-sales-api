using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.Sales;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _db;

    public SaleRepository(DefaultContext db) => _db = db;

    public async Task AddAsync(Sale sale, CancellationToken ct = default)
        => await _db.Sales.AddAsync(sale, ct);

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Sales
            .Include("_items") // usa backing field
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken ct = default)
        => await _db.Sales
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, ct);

    public async Task<IEnumerable<Sale>> ListAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var skip = (page - 1) * pageSize;
        return await _db.Sales
            .AsNoTracking()
            .OrderByDescending(s => s.SaleDate)
            .Skip(skip).Take(pageSize)
            .ToListAsync(ct);
    }

    public Task UpdateAsync(Sale sale, CancellationToken ct = default)
    {
        _db.Sales.Update(sale);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
