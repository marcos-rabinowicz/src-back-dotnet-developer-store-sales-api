using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.ORM.DomainEvents;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Npgsql;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _ctx;
    private readonly IDomainEventDispatcher _dispatcher;

    public SaleRepository(DefaultContext ctx, IDomainEventDispatcher dispatcher)
    {
        _ctx = ctx;
        _dispatcher = dispatcher;
    }


    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _ctx.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken ct = default)
    {
        var events = DomainEventsHelper.Collect(_ctx);

        await _ctx.Sales.AddAsync(sale, ct);
        try
        {
            await _ctx.SaveChangesAsync(ct);

            if (events.Count > 0)
            {
                await _dispatcher.DispatchAsync(events, ct);
                DomainEventsHelper.Clear(_ctx);
            }

        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: "23505" })
        {

            throw new DuplicateKeyException($"SaleNumber '{sale.SaleNumber}' já existe.", ex);
        }

        return sale;
    }

    public async Task UpdateAsync(Sale sale, CancellationToken ct = default)
    {
        var events = DomainEventsHelper.Collect(_ctx);

        var entry = _ctx.Entry(sale);

        if (entry.State == EntityState.Detached)
            entry.State = EntityState.Unchanged;

        try
        {
            await _ctx.SaveChangesAsync(ct);

            if (events.Count > 0)
            {
                await _dispatcher.DispatchAsync(events, ct);
                DomainEventsHelper.Clear(_ctx);
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Conflito de concorrência ao atualizar a venda.", ex);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            throw new DuplicateKeyException("Violação de chave única ao atualizar a venda.", ex);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var events = DomainEventsHelper.Collect(_ctx);

        var entity = await _ctx.Sales
        .Include(s => s.Items)
        .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (entity is null) return;

        _ctx.Sales.Remove(entity);
        try
        {
            await _ctx.SaveChangesAsync(ct);

            if (events.Count > 0)
            {
                await _dispatcher.DispatchAsync(events, ct);
                DomainEventsHelper.Clear(_ctx);
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {

            throw new ConcurrencyException("Conflito de concorrência ao excluir a venda.", ex);
        }
    }
}
