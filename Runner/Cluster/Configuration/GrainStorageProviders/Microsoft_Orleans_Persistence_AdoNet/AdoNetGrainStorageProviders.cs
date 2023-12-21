namespace Runner.Cluster.Configuration.GrainStorageProviders.Microsoft_Orleans_Persistence_AdoNet;

public static class AdoNetGrainStorageProviders
{
    public static IEnumerable<IClusterParameter> All =>
    [
        new PostgresGrainStorageProvider()
    ];
}