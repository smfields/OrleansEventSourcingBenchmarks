using EventStore.Client;
using Testcontainers.EventStoreDb;
using Testcontainers.PostgreSql;

namespace Runner.Cluster.Parameters.EventStorage;

public class MartenEventStorageProvider : ClusterParameter, IEventStorageProvider
{
    private PostgreSqlContainer? Container { get; set; }

    public override async ValueTask Initialize()
    {
        Container = new PostgreSqlBuilder().Build();
        await Container.StartAsync();
    }
    
    public override void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMartenEventStorageAsDefault(cfg =>
        {
            cfg.StoreOptions = storeOptions => storeOptions.Connection(Container!.GetConnectionString());
        });
    }

    public override async ValueTask DisposeAsync()
    {
        if (Container is not null)
        {
            await Container.StopAsync();
            await Container.DisposeAsync();
        }
    }

    public override string ToString()
    {
        return "Marten";
    }
}