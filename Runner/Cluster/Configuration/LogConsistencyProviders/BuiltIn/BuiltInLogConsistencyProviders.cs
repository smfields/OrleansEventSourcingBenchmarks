namespace Runner.Cluster.Configuration.LogConsistencyProviders.BuiltIn;

public static class BuiltInLogConsistencyProviders
{
    public static IEnumerable<IClusterParameter> All =>
    [
        new LogStorageLogConsistencyProvider(),
        new StateStorageLogConsistencyProvider()
    ];
}