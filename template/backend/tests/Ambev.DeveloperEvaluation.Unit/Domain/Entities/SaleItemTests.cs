using System;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Unit.Domain.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    [Theory]
    [InlineData(1, 0.00)]
    [InlineData(2, 0.00)]
    [InlineData(3, 0.00)]
    [InlineData(4, 0.10)]
    [InlineData(9, 0.10)]
    [InlineData(10, 0.20)]
    [InlineData(20, 0.20)]
    public void ctor_SetsDiscountAccordingToQuantity(int qty, decimal expectedDiscount)
    {
        var p = IdentitiesFactory.Product();
        var item = new SaleItem(p, qty, 10m);

        item.DiscountPercent.Should().Be(expectedDiscount);
    }

    [Fact]
    public void ctor_QuantityAbove20_ThrowsDomainException()
    {
        var p = IdentitiesFactory.Product();
        var act = () => new SaleItem(p, 21, 10m);

        act.Should().Throw<DomainException>()
           .WithMessage("*above 20*");
    }

    [Fact]
    public void LineTotal_RespectsDiscountAndCancellation()
    {
        var p = IdentitiesFactory.Product();
        var i = new SaleItem(p, 10, 10m); // 20% de desconto

        i.LineTotal.Should().Be(80m); // 10 * 10 * 0.8

        i.Cancel();
        i.LineTotal.Should().Be(0m);
    }

    [Fact]
    public void ChangePricing_RecalculatesDiscount()
    {
        var p = IdentitiesFactory.Product();
        var i = new SaleItem(p, 4, 10m); // 10%

        i.ChangePricing(9, 10m);
        i.DiscountPercent.Should().Be(0.10m);

        i.ChangePricing(10, 10m);
        i.DiscountPercent.Should().Be(0.20m);
    }
}
