using Runner.Cluster.Configuration.EventStorageProviders.Orleans_EventSourcing_EventStorage;

namespace Runner.Cluster.Configuration.EventStorageProviders;

public static class EventStorageProviders
{
    public static IEnumerable<IClusterParameter> Orleans_EventSourcing_EventStorage =>
        Orleans_EventSourcing_EventStorage_LogConsistencyProviders.All;
}