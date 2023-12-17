using EventStore.Client;
using Orleans.Configuration;
using Testcontainers.EventStoreDb;

namespace Runner.Cluster.Parameters.EventStorage;

public class EventStoreEventStorageProvider : ClusterParameter, IEventStorageProvider
{
    private EventStoreDbContainer? Container { get; set; }

    public override async ValueTask Initialize()
    {
        Container = new EventStoreDbBuilder()
            .WithImage("eventstore/eventstore:lts")
            .Build();
        await Container.StartAsync();
    }

    public override void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddEventStoreEventStorageAsDefault(cfg =>
        {
            cfg.ClientSettings = EventStoreClientSettings.Create(Container!.GetConnectionString());
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
        return "EventStore";
    }
}