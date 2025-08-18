namespace Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

internal static class CancelItemValidator
{
    public static void EnsureValid(CancelItemCommand cmd)
    {
        if (cmd.SaleId == Guid.Empty) throw new ArgumentException("SaleId is required.");
        if (cmd.ItemId == Guid.Empty) throw new ArgumentException("ItemId is required.");
    }
}
