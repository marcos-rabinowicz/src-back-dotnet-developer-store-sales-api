namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

internal static class CancelSaleValidator
{
    public static void EnsureValid(CancelSaleCommand cmd)
    {
        if (cmd.Id == Guid.Empty) throw new ArgumentException("Id is required.", nameof(cmd.Id));
    }
}
