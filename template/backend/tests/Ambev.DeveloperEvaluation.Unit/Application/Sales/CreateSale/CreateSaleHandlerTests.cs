using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

public class CreateSaleHandlerTests
{
    private static IMapper Mapper() =>
        Application.TestData.MapperFactory.New(new CreateSaleProfile());

    [Fact]
    public async Task Handle_ValidCommand_PersistsAndReturnsResult()
    {
        // arrange
        var repo = new Mock<ISaleRepository>();
        var handler = new CreateSaleHandler(repo.Object, Mapper());
        var cmd = Application.TestData.CreateSaleCommandFactory.Default();

        // act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.TotalAmount.Should().Be(30m + 36m + 80m); // 146
        result.Status.Should().Be("Active");
        repo.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Sale>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ThrowsArgumentException()
    {
        var repo = new Mock<ISaleRepository>();
        var handler = new CreateSaleHandler(repo.Object, Mapper());

        var bad = new CreateSaleCommand("", default, default, "", default, "", new CreateSaleItemDto[0]);

        var act = async () => await handler.Handle(bad, CancellationToken.None);
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
