namespace Runner.Cluster.Configuration.LogConsistencyProviders.Orleans_EventSourcing_EventStorage;

public class EventStorageLogConsistencyProvider : IClusterParameter
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddEventStorageBasedLogConsistencyProviderAsDefault();    
    }

    public override string ToString() => "EventStorage";
}