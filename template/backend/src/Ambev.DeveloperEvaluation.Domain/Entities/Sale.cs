using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale
{
    public Guid Id { get; }
    public string SaleNumber { get; private set; }
    public DateTime Date { get; private set; }
    public CustomerIdentity Customer { get; private set; }
    public BranchIdentity Branch { get; private set; }
    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
    public SaleStatus Status { get; private set; }

    public decimal TotalAmount => Math.Round(_items.Sum(i => i.LineTotal), 2);

    private readonly List<object> _domainEvents = new();
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    private Sale() { } // ORM

    public Sale(string saleNumber, DateTime date, CustomerIdentity customer, BranchIdentity branch)
    {
        Id = Guid.NewGuid();
        UpdateHeader(saleNumber, date, customer, branch);
        Status = SaleStatus.Active;
        AddEvent(new SaleCreatedEvent(Id, saleNumber, date, customer.Id, branch.Id));
    }

    public void UpdateHeader(string saleNumber, DateTime date, CustomerIdentity customer, BranchIdentity branch)
    {
        if (string.IsNullOrWhiteSpace(saleNumber))
            throw new DomainException("Sale number is required.");

        SaleNumber = saleNumber.Trim();
        Date = date;
        Customer = customer;
        Branch = branch;

        AddEvent(new SaleModifiedEvent(Id));
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

    private void EnsureNotCancelled()
    {
        if (Status == SaleStatus.Cancelled)
            throw new DomainException("Sale is cancelled.");
    }

    private void AddEvent(object @event) => _domainEvents.Add(@event);
}
