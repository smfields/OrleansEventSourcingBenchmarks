namespace Runner.Parameters.LogConsistencyProviders;

public class StateStorageLogConsistencyProvider : IClusterParameter
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddLogStorageBasedLogConsistencyProviderAsDefault();
    }
    
    public override string ToString() => "StateStorage";
}