using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using FluentAssertions;
using Xunit;

[CollectionDefinition("pg-it")]
public class PgItCollection : ICollectionFixture<PostgresTestContainerFixture> { }

[Collection("pg-it")]
public class OrderRepositoryTests
{
    private readonly PostgresTestContainerFixture _fx;

    public OrderRepositoryTests(PostgresTestContainerFixture fx) => _fx = fx;

    [Fact]
    public async Task Should_Add_And_List_With_Paging_And_Search()
    {
        using var ctx = OrmContextFactory.Create(_fx.ConnectionString);
        var repo = new OrderRepository(ctx);

        var o1 = await repo.AddAsync(new Order
        {
            Items = { new OrderItem { Description = "IPA", UnitPrice = 15, Quantity = 5 } }
        });

        _ = await repo.AddAsync(new Order
        {
            Items = { new OrderItem { Description = "Lager", UnitPrice = 12, Quantity = 2 } }
        });

        var (items, total) = await repo.ListAsync(1, 10, "ipa", "Total", true);
        total.Should().BeGreaterOrEqualTo(1);
        items.Should().ContainSingle(x => x.Id == o1.Id);
    }
}