namespace Runner.Cluster.Parameters.LogConsistencyProviders;

public class EventStorageLogConsistencyProvider : ClusterParameter, IEventStorageLogConsistencyProvider
{
    public override void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddEventStorageBasedLogConsistencyProviderAsDefault();    
    }

    public override string ToString() => "EventStorage";
}