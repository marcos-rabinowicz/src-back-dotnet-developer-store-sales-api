using System;
using System.Collections.Generic;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string SaleNumber { get; private set; } = default!;
    public DateTime SaleDate { get; private set; }

    // External Identities + snapshots
    public string CustomerId { get; private set; } = default!;
    public string CustomerNameSnapshot { get; private set; } = default!;
    public string BranchId { get; private set; } = default!;
    public string BranchNameSnapshot { get; private set; } = default!;

    public SaleStatus Status { get; private set; } = SaleStatus.Active;

    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    public decimal TotalAmount => Math.Round(_items.Where(i => !i.Cancelled).Sum(i => i.Total), 2);

    protected Sale() { } // EF

    public Sale(
        string saleNumber, DateTime saleDate,
        string customerId, string customerNameSnapshot,
        string branchId, string branchNameSnapshot)
    {
        if (string.IsNullOrWhiteSpace(saleNumber)) throw new ArgumentException("saleNumber required");
        if (string.IsNullOrWhiteSpace(customerId)) throw new ArgumentException("customerId required");
        if (string.IsNullOrWhiteSpace(customerNameSnapshot)) throw new ArgumentException("customerNameSnapshot required");
        if (string.IsNullOrWhiteSpace(branchId)) throw new ArgumentException("branchId required");
        if (string.IsNullOrWhiteSpace(branchNameSnapshot)) throw new ArgumentException("branchNameSnapshot required");

        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        CustomerNameSnapshot = customerNameSnapshot;
        BranchId = branchId;
        BranchNameSnapshot = branchNameSnapshot;
    }

    public SaleItem AddItem(string productId, string productNameSnapshot, int quantity, decimal unitPrice)
    {
        EnsureActive();
        var item = new SaleItem(productId, productNameSnapshot, quantity, unitPrice);
        _items.Add(item);
        return item;
    }

    public void UpdateItem(Guid itemId, int quantity, decimal unitPrice)
    {
        EnsureActive();
        var item = _items.Single(i => i.Id == itemId);
        item.Update(quantity, unitPrice);
    }

    public void Cancel()
    {
        EnsureActive();
        Status = SaleStatus.Cancelled;
    }

    public void CancelItem(Guid itemId)
    {
        EnsureActive();
        var item = _items.Single(i => i.Id == itemId);
        item.Cancel();
    }

    private void EnsureActive()
    {
        if (Status == SaleStatus.Cancelled)
            throw new InvalidOperationException("Sale is cancelled");
    }
}
