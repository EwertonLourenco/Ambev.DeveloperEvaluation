using System.Net.Http.Json;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.Application.Orders.GetOrder;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Xunit;

public class OrdersEndpointsTests : IAsyncLifetime
{
    private PostgreSqlTestcontainer _pg = default!;
    private CustomWebAppFactory _factory = default!;
    private HttpClient _client = default!;

    public async Task InitializeAsync()
    {
        var cfg = new PostgreSqlTestcontainerConfiguration
        {
            Database = "dev_eval_func",
            Username = "developer",
            Password = "ev@luAt10n"
        };
        _pg = new TestcontainersBuilder<PostgreSqlTestcontainer>().WithDatabase(cfg).Build();
        await _pg.StartAsync();

        _factory = new CustomWebAppFactory(_pg.ConnectionString);
        _client = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        _factory.Dispose();
        await _pg.DisposeAsync();
    }

    [Fact]
    public async Task Should_Create_And_Get_Order()
    {
        var payload = new CreateOrderCommand(new[]
        {
            new CreateOrderItemDto("Weiss 500ml", 12.5m, 5)
        });

        var create = await _client.PostAsJsonAsync("/api/orders", payload);
        create.EnsureSuccessStatusCode();
        var created = await create.Content.ReadFromJsonAsync<CreateOrderResult>();

        created!.Total.Should().Be(56.25m);

        var get = await _client.GetAsync($"/api/orders/{created.Id}");
        get.EnsureSuccessStatusCode();
        var detail = await get.Content.ReadFromJsonAsync<GetOrderResult>();
        detail!.Items.Should().HaveCount(1);
    }
}