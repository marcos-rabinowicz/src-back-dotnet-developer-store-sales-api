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
    public void Quantity_OneAndTwenty_AreValid()
    {
        var v = new SaleItemValidator();
        var item1 = new SaleItem(IdentitiesFactory.Product(), 1, 10m);
        var item20 = new SaleItem(IdentitiesFactory.Product(), 20, 10m);

        v.Validate(item1).IsValid.Should().BeTrue();
        v.Validate(item20).IsValid.Should().BeTrue();
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
