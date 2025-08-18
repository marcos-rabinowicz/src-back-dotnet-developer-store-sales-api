namespace Ambev.DeveloperEvaluation.Application.Sales.Items.ChangeItem;

internal static class ChangeItemValidator
{
    public static void EnsureValid(ChangeItemCommand cmd)
    {
        if (cmd.SaleId == Guid.Empty) throw new ArgumentException("SaleId is required.");
        if (cmd.ItemId == Guid.Empty) throw new ArgumentException("ItemId is required.");
        if (cmd.Quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (cmd.UnitPrice < 0) throw new ArgumentException("Unit price cannot be negative.");
    }
}
