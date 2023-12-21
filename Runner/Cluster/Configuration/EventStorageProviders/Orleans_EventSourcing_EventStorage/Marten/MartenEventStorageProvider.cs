using Testcontainers.PostgreSql;

namespace Runner.Cluster.Configuration.EventStorageProviders.Orleans_EventSourcing_EventStorage.Marten;

public class MartenEventStorageProvider : IClusterParameter
{
    private PostgreSqlContainer? Container { get; set; }

    public async ValueTask Initialize()
    {
        Container = new PostgreSqlBuilder().Build();
        await Container.StartAsync();
    }
    
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMartenEventStorageAsDefault(cfg =>
        {
            cfg.StoreOptions = storeOptions => storeOptions.Connection(Container!.GetConnectionString());
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (Container is not null)
        {
            await Container.StopAsync();
            await Container.DisposeAsync();
        }
    }

    public override string ToString() => "Marten";
}