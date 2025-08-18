namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

internal static class AddItemValidator
{
    public static void EnsureValid(AddItemCommand cmd)
    {
        if (cmd.SaleId == Guid.Empty) throw new ArgumentException("SaleId is required.", nameof(cmd.SaleId));
        if (cmd.ProductId == Guid.Empty || string.IsNullOrWhiteSpace(cmd.ProductName))
            throw new ArgumentException("Product is required.");
        if (cmd.Quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (cmd.UnitPrice < 0) throw new ArgumentException("Unit price cannot be negative.");
    }
}
