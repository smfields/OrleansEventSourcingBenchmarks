using Runner.Parameters.EventStorage;
using Runner.Parameters.GrainStorage;
using Runner.Parameters.LogConsistencyProviders;

namespace Runner.Parameters;

public static class ParameterConstants
{
    public static IClusterParameter[] EventStorageLogConsistencyProviders =>
    [
        new EventStorageLogConsistencyProvider()
    ];
    
    public static IClusterParameter[] EventStorageProviders => 
    [
        new MemoryEventStorageProvider(),
        new EventStoreEventStorageProvider(),
        new MartenEventStorageProvider()
    ];
    
    public static IClusterParameter[] GrainStorageLogConsistencyProviders =>
    [
        new LogStorageLogConsistencyProvider(),
        new StateStorageLogConsistencyProvider()
    ];
    
    public static IClusterParameter[] GrainStorageProviders => 
    [
        new MemoryGrainStorageProvider(),
        new PostgresGrainStorage(),
        new RedisGrainStorage()
    ];
}