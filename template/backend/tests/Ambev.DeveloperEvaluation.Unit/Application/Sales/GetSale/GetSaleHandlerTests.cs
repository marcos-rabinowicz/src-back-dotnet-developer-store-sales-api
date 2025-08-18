using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSale;

public class GetSaleHandlerTests
{
    private static IMapper Mapper() =>
        Application.TestData.MapperFactory.New(new GetSaleProfile());

    [Fact]
    public async Task Handle_Found_ReturnsMappedDto()
    {
        // arrange – uma venda com 2 itens
        var sale = new Sale("S-9", System.DateTime.UtcNow,
            CustomerIdentity.Create(System.Guid.NewGuid(), "Cust"),
            BranchIdentity.Create(System.Guid.NewGuid(), "Branch"));
        sale.AddItem(ProductIdentity.Create(System.Guid.NewGuid(), "Beer A"), 4, 10m);  // 36
        sale.AddItem(ProductIdentity.Create(System.Guid.NewGuid(), "Beer B"), 10, 5m);  // 40

        var repo = new Mock<ISaleRepository>();
        repo.Setup(r => r.GetByIdAsync(sale.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sale);

        var handler = new GetSaleHandler(repo.Object, Mapper());

        // act
        var result = await handler.Handle(new GetSaleCommand(sale.Id), CancellationToken.None);

        // assert
        result.Id.Should().Be(sale.Id);
        result.Items.Should().HaveCount(2);
        result.TotalAmount.Should().Be(36m + 40m);
    }

    [Fact]
    public async Task Handle_NotFound_Throws()
    {
        var repo = new Mock<ISaleRepository>();
        var handler = new GetSaleHandler(repo.Object, Mapper());

        var act = async () => await handler.Handle(new GetSaleCommand(System.Guid.NewGuid()), CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
