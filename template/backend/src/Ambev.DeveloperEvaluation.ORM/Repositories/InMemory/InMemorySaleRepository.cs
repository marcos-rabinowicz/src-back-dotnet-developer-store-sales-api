using System.Collections.Concurrent;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.InMemory;

public class InMemorySaleRepository : ISaleRepository
{
    private static readonly ConcurrentDictionary<Guid, Sale> _store = new();

    public Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _store.TryGetValue(id, out var sale);
        return Task.FromResult(sale);
    }

    public Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _store[sale.Id] = sale;
        return Task.FromResult(sale);
    }

    public Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _store[sale.Id] = sale;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _store.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}
