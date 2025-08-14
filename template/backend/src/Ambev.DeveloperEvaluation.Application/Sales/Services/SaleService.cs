using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<SaleService> _logger;

    public SaleService(ISaleRepository repo, IMapper mapper, ILogger<SaleService> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SaleResponseDto> CreateAsync(CreateSaleRequestDto req, CancellationToken ct = default)
    {
        // Garantir unicidade do número da venda
        var exists = await _repo.GetBySaleNumberAsync(req.SaleNumber, ct);
        if (exists is not null) throw new InvalidOperationException("SaleNumber already exists");

        var sale = new Sale(
            req.SaleNumber, req.SaleDate,
            req.Customer.Id, req.Customer.NameSnapshot,
            req.Branch.Id, req.Branch.NameSnapshot);

        foreach (var it in req.Items)
            sale.AddItem(it.ProductId, it.ProductNameSnapshot, it.Quantity, it.UnitPrice);

        await _repo.AddAsync(sale, ct);
        await _repo.SaveChangesAsync(ct);

        _logger.LogInformation("DomainEvent: SaleCreated {SaleId} {SaleNumber} at {At}", sale.Id, sale.SaleNumber, DateTime.UtcNow);

        return _mapper.Map<SaleResponseDto>(sale);
    }

    public async Task<SaleResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var sale = await _repo.GetByIdAsync(id, ct);
        return sale is null ? null : _mapper.Map<SaleResponseDto>(sale);
    }

    public async Task<IEnumerable<SaleResponseDto>> ListAsync(int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var list = await _repo.ListAsync(page, pageSize, ct);
        return list.Select(_mapper.Map<SaleResponseDto>);
    }

    public async Task<SaleResponseDto> UpdateAsync(Guid id, CreateSaleRequestDto req, CancellationToken ct = default)
    {
        var sale = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Sale not found");

        // Estratégia simples: cancelar itens antigos e adicionar novos
        foreach (var old in sale.Items.ToList())
            if (!old.Cancelled) old.Cancel();

        foreach (var it in req.Items)
            sale.AddItem(it.ProductId, it.ProductNameSnapshot, it.Quantity, it.UnitPrice);

        await _repo.UpdateAsync(sale, ct);
        await _repo.SaveChangesAsync(ct);

        _logger.LogInformation("DomainEvent: SaleModified {SaleId} {SaleNumber} at {At}", sale.Id, sale.SaleNumber, DateTime.UtcNow);

        return _mapper.Map<SaleResponseDto>(sale);
    }

    public async Task<SaleResponseDto> CancelAsync(Guid id, CancellationToken ct = default)
    {
        var sale = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Sale not found");
        sale.Cancel();

        await _repo.UpdateAsync(sale, ct);
        await _repo.SaveChangesAsync(ct);

        _logger.LogInformation("DomainEvent: SaleCancelled {SaleId} {SaleNumber} at {At}", sale.Id, sale.SaleNumber, DateTime.UtcNow);

        return _mapper.Map<SaleResponseDto>(sale);
    }

    public async Task<SaleResponseDto> CancelItemAsync(Guid id, Guid itemId, CancellationToken ct = default)
    {
        var sale = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Sale not found");
        sale.CancelItem(itemId);

        await _repo.UpdateAsync(sale, ct);
        await _repo.SaveChangesAsync(ct);

        _logger.LogInformation("DomainEvent: ItemCancelled {SaleId} {ItemId} at {At}", sale.Id, itemId, DateTime.UtcNow);

        return _mapper.Map<SaleResponseDto>(sale);
    }
}
