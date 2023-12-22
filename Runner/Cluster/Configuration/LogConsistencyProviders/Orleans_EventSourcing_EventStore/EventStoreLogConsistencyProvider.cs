using EventStore.Client;
using Testcontainers.EventStoreDb;

namespace Runner.Cluster.Configuration.LogConsistencyProviders.Orleans_EventSourcing_EventStore;

public class EventStoreLogConsistencyProvider : IClusterParameter
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
        siloBuilder.AddEventStoreBasedLogConsistencyProviderAsDefault(opts =>
        {
            opts.ClientSettings = EventStoreClientSettings.Create(Container!.GetConnectionString());
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