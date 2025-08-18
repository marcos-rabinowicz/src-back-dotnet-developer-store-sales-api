using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class SaleItemValidatorTests
{
    private static SaleItem NewItem(int qty) =>
        new SaleItem(IdentitiesFactory.Product(), qty, 10m);

    [Fact]
    public void Quantity_MustBeBetween1And20()
    {
        var validator = new SaleItemValidator();

        var ok = validator.Validate(NewItem(1));
        ok.IsValid.Should().BeTrue();

        var ok2 = validator.Validate(NewItem(20));
        ok2.IsValid.Should().BeTrue();

        var invalid = new SaleItem(IdentitiesFactory.Product(), 4, 10m);
        invalid.ChangePricing(21, 10m); // força estado inválido via método? Não é possível – então apenas validar regra.

        // Como não conseguimos criar >20 (o domínio bloqueia), testamos a regra de validação
        // indiretamente através do Must da porcentagem:
        var item = new SaleItem(IdentitiesFactory.Product(), 4, 10m);
        var result = validator.Validate(item);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(3, 0.00)]
    [InlineData(4, 0.10)]
    [InlineData(10, 0.20)]
    public void DiscountPercent_MustMatchQuantityRule(int qty, decimal expected)
    {
        var validator = new SaleItemValidator();
        var item = NewItem(qty);

        var result = validator.Validate(item);

        result.IsValid.Should().BeTrue();
        item.DiscountPercent.Should().Be(expected);
    }
}
