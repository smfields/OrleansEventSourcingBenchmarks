namespace Runner.Cluster.Parameters.LogConsistencyProviders;

public class LogStorageLogConsistencyProvider : ClusterParameter, IGrainStorageBasedLogConsistencyProvider
{
    public override void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddLogStorageBasedLogConsistencyProviderAsDefault();
    }
    
    public override string ToString() => "LogStorage";
}