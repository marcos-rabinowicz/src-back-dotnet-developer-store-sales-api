using System;
using System.Threading;
using System.Threading.Tasks;
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
    [Fact]
    public async Task Handle_AddsItemAndReturnsLineAndSaleTotals()
    {
        // arrange
        Sale sale = new Sale("S-2", DateTime.UtcNow,
            CustomerIdentity.Create(Guid.NewGuid(), "Cust"),
            BranchIdentity.Create(Guid.NewGuid(), "Branch"));

        Mock<ISaleRepository> repo = new Mock<ISaleRepository>();
        repo.Setup(r => r.GetByIdAsync(sale.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sale);

        AddItemHandler handler = new AddItemHandler(repo.Object);

        AddItemCommand cmd = new AddItemCommand(
            SaleId: sale.Id,
            ProductId: Guid.NewGuid(),
            ProductName: "Beer X",
            Quantity: 10,
            UnitPrice: 10m);

        // act
        AddItemResult result = await handler.Handle(cmd, CancellationToken.None);

        // assert
        result.LineTotal.Should().Be(80m);
        result.SaleTotalAmount.Should().Be(80m);
        repo.Verify(r => r.UpdateAsync(sale, It.IsAny<CancellationToken>()), Times.Once);
    }
}
