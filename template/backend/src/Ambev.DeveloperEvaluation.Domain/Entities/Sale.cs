using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : IHasDomainEvents
{
    public Guid Id { get; }
    public string? SaleNumber { get; private set; }
    public DateTime Date { get; private set; }
    public SaleStatus Status { get; private set; }
    public CustomerIdentity? Customer { get; private set; }
    public BranchIdentity? Branch { get; private set; }
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public decimal TotalAmount => Math.Round(_items.Sum(i => i.LineTotal), 2);
    private readonly List<SaleItem> _items = [];
    private readonly List<IDomainEvent> _domainEvents = [];

    private Sale() { }

    public Sale(string saleNumber, DateTime date, CustomerIdentity customer, BranchIdentity branch)
    {
        Id = Guid.NewGuid();
        SetHeader(saleNumber, date, customer, branch);
        Status = SaleStatus.Active;
        AddEvent(new SaleCreatedEvent(Id, saleNumber, date, customer.Id, branch.Id));
    }

    public SaleItem AddItem(ProductIdentity product, int quantity, decimal unitPrice)
    {
        EnsureNotCancelled();
        var item = new SaleItem(product, quantity, unitPrice);
        _items.Add(item);
        AddEvent(new SaleModifiedEvent(Id));
        return item;
    }

    public void ChangeItem(Guid itemId, int quantity, decimal unitPrice)
    {
        EnsureNotCancelled();
        var item = _items.FirstOrDefault(i => i.Id == itemId)
                   ?? throw new DomainException("Item not found.");
        item.ChangePricing(quantity, unitPrice);
        AddEvent(new SaleModifiedEvent(Id));
    }

    public void CancelItem(Guid itemId)
    {
        EnsureNotCancelled();
        var item = _items.FirstOrDefault(i => i.Id == itemId)
                   ?? throw new DomainException("Item not found.");
        item.Cancel();
        AddEvent(new ItemCancelledEvent(Id, itemId));
    }

    public void Cancel()
    {
        EnsureNotCancelled();
        Status = SaleStatus.Cancelled;
        AddEvent(new SaleCancelledEvent(Id));
    }

    public void UpdateHeader(string saleNumber, DateTime date, CustomerIdentity customer, BranchIdentity branch)
    {
        EnsureNotCancelled();

        var trimmed = saleNumber?.Trim();
        bool changed =
            !string.Equals(SaleNumber, trimmed, StringComparison.Ordinal) ||
            Date != date ||
            Customer?.Id != customer.Id ||
            Branch?.Id != branch.Id;

        if (!changed) return;

        SetHeader(trimmed!, date, customer, branch);
        AddEvent(new SaleModifiedEvent(Id));
    }

    private void SetHeader(string saleNumber, DateTime date, CustomerIdentity customer, BranchIdentity branch)
    {
        if (string.IsNullOrWhiteSpace(saleNumber))
            throw new DomainException("Sale number is required.");

        SaleNumber = saleNumber.Trim();
        Date = date;
        Customer = customer;
        Branch = branch;
    }

    private void EnsureNotCancelled()
    {
        if (Status == SaleStatus.Cancelled)
            throw new DomainException("Sale is cancelled.");
    }

    public void ClearDomainEvents() => _domainEvents.Clear();

    private void AddEvent(IDomainEvent @event) => _domainEvents.Add(@event);
}
