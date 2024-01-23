using Runner.Configuration.Core;

namespace Runner.Configuration.Parameters.LogConsistency.BuiltIn;

[Parameter(LogConsistencyProvider.ParameterName, LogConsistencyProvider.StateStorage)]
public class StateStorageLogConsistencyProvider : IConfigureSilo
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddLogStorageBasedLogConsistencyProviderAsDefault();
    }
}