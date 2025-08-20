using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem
{
    private const int MaxQuantityPerProduct = 20;

    public Guid Id { get; private set; }
    public ProductIdentity? Product { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    /// <summary>0.10m = 10% | 0.20m = 20% — sempre derivado da quantidade.</summary>
    public decimal DiscountPercent { get; private set; }
    public bool IsCancelled { get; private set; }

    public decimal LineTotal => IsCancelled
        ? 0m
        : Math.Round(Quantity * UnitPrice * (1 - DiscountPercent), 2);

    private SaleItem() { }

    public SaleItem(ProductIdentity product, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        Product = product;
        ChangePricing(quantity, unitPrice);
    }

    public void ChangePricing(int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");
        if (quantity > MaxQuantityPerProduct)
            throw new DomainException($"It's not possible to sell above {MaxQuantityPerProduct} identical items.");
        if (unitPrice < 0)
            throw new DomainException("Unit price cannot be negative.");

        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountPercent = CalculateDiscountPercent(quantity);
    }

    public void Cancel() => IsCancelled = true;

    private static decimal CalculateDiscountPercent(int quantity)
    {
        if (quantity >= 10) return 0.20m;
        if (quantity >= 4) return 0.10m;
        return 0.00m;
    }
}
