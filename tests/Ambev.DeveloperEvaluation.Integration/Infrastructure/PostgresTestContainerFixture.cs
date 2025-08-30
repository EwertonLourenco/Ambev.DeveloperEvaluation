using System;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Integration;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Xunit;

public class PostgresTestContainerFixture : IAsyncLifetime
{
    public PostgreSqlTestcontainer? Container { get; private set; }
    public string ConnectionString { get; private set; } = default!;
    private bool _usingExternalDb;

    public async Task InitializeAsync()
    {
        var cs = TestConfig.GetConnectionString();

        if (!string.IsNullOrWhiteSpace(cs) && cs.IndexOf("Port=55432", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            ConnectionString = cs;
            _usingExternalDb = true;
            return;
        }

        _usingExternalDb = false;

        var cfg = new PostgreSqlTestcontainerConfiguration
        {
            Database = "developer_evaluation_it",
            Username = "developer",
            Password = "ev@luAt10n"
        };

        Container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(cfg)
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .Build();

        await Container.StartAsync();
        ConnectionString = Container.ConnectionString;
    }

    public async Task DisposeAsync()
    {
        if (!_usingExternalDb && Container is not null)
        {
            await Container.DisposeAsync();
        }
    }
}