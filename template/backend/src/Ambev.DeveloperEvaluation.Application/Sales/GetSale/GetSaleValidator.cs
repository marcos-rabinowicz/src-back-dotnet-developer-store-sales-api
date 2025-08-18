namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

internal static class GetSaleValidator
{
    public static void EnsureValid(GetSaleCommand cmd)
    {
        if (cmd.Id == Guid.Empty) throw new ArgumentException("Id is required.", nameof(cmd.Id));
    }
}
