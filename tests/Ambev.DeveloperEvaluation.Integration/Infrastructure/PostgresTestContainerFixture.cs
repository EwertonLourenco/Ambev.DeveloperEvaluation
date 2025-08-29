using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Xunit;

public class PostgresTestContainerFixture : IAsyncLifetime
{
    public PostgreSqlTestcontainer Container { get; private set; } = default!;
    public string ConnectionString => Container.ConnectionString;

    public async Task InitializeAsync()
    {
        var cfg = new PostgreSqlTestcontainerConfiguration
        {
            Database = "developer_evaluation_it",
            Username = "developer",
            Password = "ev@luAt10n"
        };

        Container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(cfg)
            .Build();

        await Container.StartAsync();
    }

    public async Task DisposeAsync() => await Container.DisposeAsync();
}