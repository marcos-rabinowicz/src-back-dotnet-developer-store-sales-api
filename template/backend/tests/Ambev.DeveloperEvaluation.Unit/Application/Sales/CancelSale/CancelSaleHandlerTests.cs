using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CancelSale;

public class CancelSaleHandlerTests
{
    [Fact]
    public async Task Handle_CancelsAndPersists()
    {
        var sale = new Sale("S-1", System.DateTime.UtcNow,
            CustomerIdentity.Create(System.Guid.NewGuid(), "Cust"),
            BranchIdentity.Create(System.Guid.NewGuid(), "Branch"));

        var repo = new Mock<ISaleRepository>();
        repo.Setup(r => r.GetByIdAsync(sale.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sale);

        var handler = new CancelSaleHandler(repo.Object);

        var result = await handler.Handle(new CancelSaleCommand(sale.Id), CancellationToken.None);

        result.Status.Should().Be("Cancelled");
        repo.Verify(r => r.UpdateAsync(sale, It.IsAny<CancellationToken>()), Times.Once);
    }
}
