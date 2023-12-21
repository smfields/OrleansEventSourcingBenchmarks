using EventStore.Client;
using Testcontainers.EventStoreDb;

namespace Runner.Cluster.Configuration.EventStorageProviders.Orleans_EventSourcing_EventStorage.EventStore;

public class EventStoreEventStorageProvider : IClusterParameter, IAsyncDisposable
{
    private EventStoreDbContainer? Container { get; set; }

    public async ValueTask Initialize()
    {
        Container = new EventStoreDbBuilder()
            .WithImage("eventstore/eventstore:lts")
            .Build();
        
        await Container.StartAsync();
    }

    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddEventStoreEventStorageAsDefault(cfg =>
        {
            cfg.ClientSettings = EventStoreClientSettings.Create(Container!.GetConnectionString());
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

    public override string ToString() => "EventStore";
}