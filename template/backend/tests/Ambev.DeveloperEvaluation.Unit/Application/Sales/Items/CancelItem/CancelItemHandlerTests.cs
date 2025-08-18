using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Items.CancelItem;

public class CancelItemHandlerTests
{
    [Fact]
    public async Task Handle_CancelsItemAndReturnsNewTotal()
    {
        var sale = new Sale("S-4", System.DateTime.UtcNow,
            CustomerIdentity.Create(System.Guid.NewGuid(), "Cust"),
            BranchIdentity.Create(System.Guid.NewGuid(), "Branch"));
        var item = sale.AddItem(ProductIdentity.Create(System.Guid.NewGuid(), "Beer Z"), 10, 10m); // 80

        var repo = new Mock<ISaleRepository>();
        repo.Setup(r => r.GetByIdAsync(sale.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sale);

        var handler = new CancelItemHandler(repo.Object);

        var result = await handler.Handle(new CancelItemCommand(sale.Id, item.Id), CancellationToken.None);

        result.SaleTotalAmount.Should().Be(0m);
        repo.Verify(r => r.UpdateAsync(sale, It.IsAny<CancellationToken>()), Times.Once);
    }
}
