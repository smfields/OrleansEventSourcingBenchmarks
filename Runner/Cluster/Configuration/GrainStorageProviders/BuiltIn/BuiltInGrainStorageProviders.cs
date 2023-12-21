using Runner.Cluster.Configuration.GrainStorageProviders.Microsoft_Orleans_Persistence_AdoNet;
using Runner.Cluster.Configuration.GrainStorageProviders.Microsoft_Orleans_Persistence_Redis;

namespace Runner.Cluster.Configuration.GrainStorageProviders.BuiltIn;

public static class BuiltInGrainStorageProviders
{
    public static IEnumerable<IClusterParameter> All => 
    [
        new MemoryGrainStorageProvider(),
        new RedisGrainStorageProvider(),
        new PostgresGrainStorageProvider()
    ];
}