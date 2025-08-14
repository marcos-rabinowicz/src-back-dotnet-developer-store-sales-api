using System;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    // External Identity + denormalização
    public string ProductId { get; private set; } = default!;
    public string ProductNameSnapshot { get; private set; } = default!;

    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    /// <summary>0.10 = 10%, 0.20 = 20%</summary>
    public decimal DiscountPercent { get; private set; }

    public bool Cancelled { get; private set; }

    public decimal Total => Math.Round(Quantity * UnitPrice * (1 - DiscountPercent), 2);

    protected SaleItem() { } // EF

    public SaleItem(string productId, string productNameSnapshot, int quantity, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(productId)) throw new ArgumentException("productId required");
        if (string.IsNullOrWhiteSpace(productNameSnapshot)) throw new ArgumentException("productNameSnapshot required");
        if (quantity <= 0) throw new ArgumentException("quantity must be > 0");
        if (unitPrice < 0) throw new ArgumentException("unitPrice must be >= 0");

        ProductId = productId;
        ProductNameSnapshot = productNameSnapshot;

        SetQuantity(quantity);
        UnitPrice = unitPrice;
    }

    public void Update(int quantity, decimal unitPrice)
    {
        EnsureNotCancelled();

        if (unitPrice < 0) throw new ArgumentException("unitPrice must be >= 0");

        SetQuantity(quantity);
        UnitPrice = unitPrice;
    }

    public void Cancel() => Cancelled = true;

    private void EnsureNotCancelled()
    {
        if (Cancelled) throw new InvalidOperationException("Item is cancelled");
    }

    private void SetQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("quantity must be > 0");
        if (quantity > 20) throw new InvalidOperationException("Cannot sell more than 20 identical items");

        Quantity = quantity;
        DiscountPercent = CalculateDiscount(quantity);
    }

    public static decimal CalculateDiscount(int quantity)
        => quantity >= 10 ? 0.20m
         : quantity >= 4  ? 0.10m
         : 0.00m;
}
