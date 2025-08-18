using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.ChangeItem;

public class ChangeItemRequestValidator : AbstractValidator<ChangeItemRequest>
{
    public ChangeItemRequestValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}
