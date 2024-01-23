using EventStore.Client;
using Runner.Configuration.Core;
using Testcontainers.EventStoreDb;

namespace Runner.Configuration.Parameters.EventStorage.Orleans_EventSourcing_EventStorage.EventStore;

[Parameter(EventStorageProvider.ParameterName, EventStorageProvider.EventStore)]
public class EventStoreEventStorageProvider : 
    IAsyncInitializable, 
    IConfigureSilo,
    IAsyncDisposable
{
    private EventStoreDbContainer? Container { get; set; }

    public async Task InitializeAsync()
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
}