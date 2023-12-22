using Runner.Cluster.Configuration.LogConsistencyProviders.BuiltIn;
using Runner.Cluster.Configuration.LogConsistencyProviders.Orleans_EventSourcing_EventStorage;
using Runner.Cluster.Configuration.LogConsistencyProviders.Orleans_EventSourcing_EventStore;

namespace Runner.Cluster.Configuration.LogConsistencyProviders;

public static class LogConsistencyProviders
{
    public static IEnumerable<IClusterParameter> All => BuiltIn
        .Concat(Orleans_EventSourcing_EventStorage);
    
    public static IEnumerable<IClusterParameter> BuiltIn => BuiltInLogConsistencyProviders.All;

    public static IEnumerable<IClusterParameter> Orleans_EventSourcing_EventStorage =>
    [
        new EventStorageLogConsistencyProvider()
    ];

    public static IEnumerable<IClusterParameter> Orleans_EventSourcing_EventStore => 
    [
        new EventStoreLogConsistencyProvider()
    ];
}