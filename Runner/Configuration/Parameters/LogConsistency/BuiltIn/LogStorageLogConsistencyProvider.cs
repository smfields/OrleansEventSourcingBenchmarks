using Runner.Configuration.Core;

namespace Runner.Configuration.Parameters.LogConsistency.BuiltIn;

[Parameter(LogConsistencyProvider.ParameterName, LogConsistencyProvider.LogStorage)]
public class LogStorageLogConsistencyProvider : IConfigureSilo
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddLogStorageBasedLogConsistencyProviderAsDefault();
    }
}