using Runner.Cluster.Configuration.GrainStorageProviders.BuiltIn;
using Runner.Cluster.Configuration.GrainStorageProviders.Microsoft_Orleans_Persistence_AdoNet;
using Runner.Cluster.Configuration.GrainStorageProviders.Microsoft_Orleans_Persistence_Redis;

namespace Runner.Cluster.Configuration.GrainStorageProviders;

public static class GrainStorageProviders
{
    public static IEnumerable<IClusterParameter> All =>
        BuiltIn
            .Concat(AdoNet)
            .Concat(Redis);

    public static IEnumerable<IClusterParameter> BuiltIn => BuiltInGrainStorageProviders.All;

    public static IEnumerable<IClusterParameter> AdoNet => AdoNetGrainStorageProviders.All;

    public static IEnumerable<IClusterParameter> Redis => 
    [
        new RedisGrainStorageProvider()
    ];
}