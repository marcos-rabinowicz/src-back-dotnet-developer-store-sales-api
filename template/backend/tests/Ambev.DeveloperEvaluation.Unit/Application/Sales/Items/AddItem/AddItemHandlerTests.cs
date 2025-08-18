using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Items.AddItem;

public class AddItemHandlerTests
{
    private static IMapper Mapper() =>
        Application.TestData.MapperFactory.New(new AddItemProfile());

    [Fact]
    public async Task Handle_AddsItemAndReturnsLineAndSaleTotals()
    {
        var sale = new Sale("S-2", System.DateTime.UtcNow,
            CustomerIdentity.Create(System.Guid.NewGuid(), "Cust"),
            BranchIdentity.Create(System.Guid.NewGuid(), "Branch"));

        var repo = new Mock<ISaleRepository>();
        repo.Setup(r => r.GetByIdAsync(sale.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sale);

        var handler = new AddItemHandler(repo.Object, Mapper());

        var cmd = new AddItemCommand(
            SaleId: sale.Id,
            ProductId: System.Guid.NewGuid(),
            ProductName: "Beer X",
            Quantity: 10,
            UnitPrice: 10m);

        var result = await handler.Handle(cmd, CancellationToken.None);

        result.LineTotal.Should().Be(80m);
        result.SaleTotalAmount.Should().Be(80m);
        repo.Verify(r => r.UpdateAsync(sale, It.IsAny<CancellationToken>()), Times.Once);
    }
}
