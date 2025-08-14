using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken ct = default);

    Task<IEnumerable<Sale>> ListAsync(int page, int pageSize, CancellationToken ct = default);

    Task AddAsync(Sale sale, CancellationToken ct = default);
    Task UpdateAsync(Sale sale, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
