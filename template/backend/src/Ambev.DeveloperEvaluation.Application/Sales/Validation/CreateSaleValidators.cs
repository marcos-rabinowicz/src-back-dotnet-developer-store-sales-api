using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Sales.DTOs;

namespace Ambev.DeveloperEvaluation.Application.Sales.Validation;

public class ExternalRefDtoValidator : AbstractValidator<ExternalRefDto>
{
    public ExternalRefDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NameSnapshot).NotEmpty();
    }
}

public class CreateSaleItemDtoValidator : AbstractValidator<CreateSaleItemDto>
{
    public CreateSaleItemDtoValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ProductNameSnapshot).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}

public class CreateSaleRequestDtoValidator : AbstractValidator<CreateSaleRequestDto>
{
    public CreateSaleRequestDtoValidator()
    {
        RuleFor(x => x.SaleNumber).NotEmpty();
        RuleFor(x => x.SaleDate).NotEmpty();
        RuleFor(x => x.Customer).SetValidator(new ExternalRefDtoValidator());
        RuleFor(x => x.Branch).SetValidator(new ExternalRefDtoValidator());
        RuleForEach(x => x.Items).SetValidator(new CreateSaleItemDtoValidator());
        RuleFor(x => x.Items).NotEmpty();
    }
}
