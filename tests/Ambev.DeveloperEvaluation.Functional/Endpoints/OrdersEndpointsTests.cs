using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrderFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.GetOrderFeature;
using FluentAssertions;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Endpoints;

public class OrdersEndpointsTests : IAsyncLifetime
{
    private CustomWebAppFactory _factory = default!;
    private HttpClient _client = default!;

    public async Task InitializeAsync()
    {
        var cs = TestConfig.GetConnectionString();
        _factory = new CustomWebAppFactory(cs);
        _client = _factory.CreateClient();
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        _factory?.Dispose();
        await Task.CompletedTask;
    }

    [Fact]
    public async Task Should_Create_And_Get_Order()
    {
        var payload = new CreateOrderRequest
        {
            Items = new[] { new CreateOrderItemRequest("Weiss 500ml", 12.5m, 5) }
        };

        var create = await _client.PostAsJsonAsync("/api/orders", payload);
        create.EnsureSuccessStatusCode();

        var createdEnv = await create.Content.ReadFromJsonAsync<ApiResponseWithData<CreateOrderResponse>>();
        createdEnv!.Success.Should().BeTrue();

        var created = createdEnv.Data!;
        created.Total.Should().Be(56.25m);

        var get = await _client.GetAsync($"/api/orders/{created.Id}");
        get.EnsureSuccessStatusCode();

        var detailEnv = await get.Content.ReadFromJsonAsync<ApiResponseWithData<GetOrderResponse>>();
        detailEnv!.Success.Should().BeTrue();
        detailEnv.Data!.Items.Should().HaveCount(1);
    }
}