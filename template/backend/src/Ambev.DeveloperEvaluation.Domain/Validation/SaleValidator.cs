using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(s => s.SaleNumber).NotEmpty();
        RuleFor(s => s.Customer.Id).NotEmpty();
        RuleFor(s => s.Branch.Id).NotEmpty();

        RuleForEach(s => s.Items).SetValidator(new SaleItemValidator());
        RuleFor(s => s.TotalAmount).GreaterThanOrEqualTo(0);
    }
}
