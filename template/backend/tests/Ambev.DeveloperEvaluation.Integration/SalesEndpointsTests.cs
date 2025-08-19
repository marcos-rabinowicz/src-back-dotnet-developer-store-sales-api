using System.Text.Json;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Integration.Testing;

namespace Ambev.DeveloperEvaluation.Integration;

public class SalesEndpointsTests : IClassFixture<SalesWebAppFactory>
{
    private readonly HttpClient _client;

    public SalesEndpointsTests(SalesWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_Then_Get()
    {
        var create = new
        {
            saleNumber = $"S-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
            date = DateTime.UtcNow,
            customerId = Guid.NewGuid(),
            customerName = "Customer A",
            branchId = Guid.NewGuid(),
            branchName = "Branch RJ",
            items = new[]
            {
                new { productId = Guid.NewGuid(), productName = "Beer A", quantity = 3,  unitPrice = 10m },
                new { productId = Guid.NewGuid(), productName = "Beer B", quantity = 4,  unitPrice = 10m },
                new { productId = Guid.NewGuid(), productName = "Beer C", quantity = 10, unitPrice = 10m }
            }
        };

        var resp = await _client.PostAsJsonAsync("/api/sales", create);
        resp.StatusCode.Should().Be(HttpStatusCode.Created);

        JsonElement json = await resp.Content.ReadFromJsonAsync<JsonElement>();
        Guid id = json.GetProperty("id").GetGuid();

        var get = await _client.GetAsync($"/api/sales/{id}");
        get.EnsureSuccessStatusCode();
    }
}
