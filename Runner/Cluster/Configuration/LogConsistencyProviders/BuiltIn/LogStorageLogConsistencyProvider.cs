namespace Runner.Cluster.Configuration.LogConsistencyProviders.BuiltIn;

public class LogStorageLogConsistencyProvider : IClusterParameter
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddLogStorageBasedLogConsistencyProviderAsDefault();
    }
    
    public override string ToString() => "LogStorage";
}