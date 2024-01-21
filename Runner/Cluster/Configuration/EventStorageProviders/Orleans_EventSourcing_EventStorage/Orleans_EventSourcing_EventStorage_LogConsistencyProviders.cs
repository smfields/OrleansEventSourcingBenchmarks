using Runner.Cluster.Configuration.EventStorageProviders.Orleans_EventSourcing_EventStorage.BuiltIn;
using Runner.Cluster.Configuration.EventStorageProviders.Orleans_EventSourcing_EventStorage.EventStore;

namespace Runner.Cluster.Configuration.EventStorageProviders.Orleans_EventSourcing_EventStorage;

public static class Orleans_EventSourcing_EventStorage_LogConsistencyProviders
{
    public static IEnumerable<IClusterParameter> All =>
    [
        new MemoryEventStorageProvider(),
        new EventStoreEventStorageProvider()
    ];
}