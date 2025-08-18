using System;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Unit.Domain.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    private static Sale NewSale()
        => new Sale(
            saleNumber: "S-0001",
            date: DateTime.UtcNow,
            customer: IdentitiesFactory.AnyCustomer(),
            branch: IdentitiesFactory.AnyBranch()
        );

    [Fact]
    public void NewSale_ShouldStartActive_AndRaiseCreatedEvent()
    {
        var s = NewSale();

        s.Status.ToString().Should().Be("Active");
        s.DomainEvents.Should().NotBeEmpty();
        s.DomainEvents.First().GetType().Name.Should().Contain("SaleCreated");
    }

    [Fact]
    public void AddItem_AccumulatesTotalWithDiscounts()
    {
        var s = NewSale();
        // item1: 3 x 10 => sem desconto => 30
        s.AddItem(IdentitiesFactory.Product("P1"), 3, 10m);
        // item2: 4 x 10 => 10% => 36
        s.AddItem(IdentitiesFactory.Product("P2"), 4, 10m);
        // item3: 10 x 10 => 20% => 80
        s.AddItem(IdentitiesFactory.Product("P3"), 10, 10m);

        s.TotalAmount.Should().Be(30m + 36m + 80m);
        s.DomainEvents.Should().ContainSingle(e => e.GetType().Name.Contains("SaleModified"));
    }

    [Fact]
    public void CancelItem_ShouldZeroItsLineTotal_AndRaiseEvent()
    {
        var s = NewSale();
        var item = s.AddItem(IdentitiesFactory.Product(), 10, 10m); // total 80

        s.CancelItem(item.Id);

        s.TotalAmount.Should().Be(0m);
        s.DomainEvents.Should().Contain(e => e.GetType().Name.Contains("ItemCancelled"));
    }

    [Fact]
    public void CancelSale_BlocksMutations_AndRaisesEvent()
    {
        var s = NewSale();
        var it = s.AddItem(IdentitiesFactory.Product(), 4, 10m);

        s.Cancel();

        Action add = () => s.AddItem(IdentitiesFactory.Product(), 1, 10m);
        Action change = () => s.ChangeItem(it.Id, 5, 10m);
        Action cancelItem = () => s.CancelItem(it.Id);

        add.Should().Throw<DomainException>().WithMessage("*cancelled*");
        change.Should().Throw<DomainException>().WithMessage("*cancelled*");
        cancelItem.Should().Throw<DomainException>().WithMessage("*cancelled*");

        s.DomainEvents.Should().Contain(e => e.GetType().Name.Contains("SaleCancelled"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateHeader_InvalidSaleNumber_Throws(string? invalid)
    {
        var s = NewSale();
        var cust = IdentitiesFactory.AnyCustomer();
        var branch = IdentitiesFactory.AnyBranch();

        Action act = () => s.UpdateHeader(invalid!, DateTime.UtcNow, cust, branch);

        act.Should().Throw<DomainException>().WithMessage("*required*");
    }
}
