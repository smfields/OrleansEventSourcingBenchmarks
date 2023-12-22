namespace Runner.Cluster.Configuration.GrainStorageProviders.BuiltIn;

public static class BuiltInGrainStorageProviders
{
    public static IEnumerable<IClusterParameter> All => 
    [
        new MemoryGrainStorageProvider()
    ];
}