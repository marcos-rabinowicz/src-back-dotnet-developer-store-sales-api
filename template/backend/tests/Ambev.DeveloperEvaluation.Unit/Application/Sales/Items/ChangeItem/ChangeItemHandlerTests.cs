using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Application.Sales.Items.ChangeItem;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Items.ChangeItem;

public class ChangeItemHandlerTests
{
    [Fact]
    public async Task Handle_ChangesItemAndReturnsNewSaleTotal()
    {
        var sale = new Sale("S-3", System.DateTime.UtcNow,
            CustomerIdentity.Create(System.Guid.NewGuid(), "Cust"),
            BranchIdentity.Create(System.Guid.NewGuid(), "Branch"));
        var item = sale.AddItem(ProductIdentity.Create(System.Guid.NewGuid(), "Beer Y"), 4, 10m); // 36

        var repo = new Mock<ISaleRepository>();
        repo.Setup(r => r.GetByIdAsync(sale.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sale);

        var handler = new ChangeItemHandler(repo.Object);

        var cmd = new ChangeItemCommand(sale.Id, item.Id, 10, 10m); // vira 80
        var result = await handler.Handle(cmd, CancellationToken.None);

        result.SaleTotalAmount.Should().Be(80m);
        repo.Verify(r => r.UpdateAsync(sale, It.IsAny<CancellationToken>()), Times.Once);
    }
}
