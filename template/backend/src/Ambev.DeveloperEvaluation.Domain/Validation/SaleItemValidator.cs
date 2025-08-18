using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(i => i.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(20)
            .WithMessage("It's not possible to sell above 20 identical items.");

        RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0);

        RuleFor(i => i.DiscountPercent)
            .Must((i, d) => (i.Quantity < 4 && d == 0m)
                         || (i.Quantity >= 4 && i.Quantity < 10 && d == 0.10m)
                         || (i.Quantity >= 10 && i.Quantity <= 20 && d == 0.20m))
            .WithMessage("Discount percent does not match the quantity-based business rules.");
    }
}
