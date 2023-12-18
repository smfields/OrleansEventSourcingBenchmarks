namespace Runner.Parameters.LogConsistencyProviders;

public class EventStorageLogConsistencyProvider : IClusterParameter
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddEventStorageBasedLogConsistencyProviderAsDefault();    
    }

    public override string ToString() => "EventStorage";
}