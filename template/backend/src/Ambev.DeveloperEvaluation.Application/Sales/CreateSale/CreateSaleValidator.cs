namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

internal static class CreateSaleValidator
{
    public static void EnsureValid(CreateSaleCommand cmd)
    {
        if (string.IsNullOrWhiteSpace(cmd.SaleNumber))
            throw new ArgumentException("Sale number is required.", nameof(cmd.SaleNumber));
        if (cmd.CustomerId == Guid.Empty || string.IsNullOrWhiteSpace(cmd.CustomerName))
            throw new ArgumentException("Customer is required.", nameof(cmd.CustomerId));
        if (cmd.BranchId == Guid.Empty || string.IsNullOrWhiteSpace(cmd.BranchName))
            throw new ArgumentException("Branch is required.", nameof(cmd.BranchId));
        if (cmd.Items is null || cmd.Items.Count == 0)
            throw new ArgumentException("At least one item is required.", nameof(cmd.Items));
        foreach (var i in cmd.Items)
        {
            if (i.ProductId == Guid.Empty || string.IsNullOrWhiteSpace(i.ProductName))
                throw new ArgumentException("Product is required.");
            if (i.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            if (i.UnitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative.");
        }
    }
}
