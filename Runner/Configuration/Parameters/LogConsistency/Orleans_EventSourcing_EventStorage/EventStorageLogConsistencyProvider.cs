using Runner.Configuration.Core;

namespace Runner.Configuration.Parameters.LogConsistency.Orleans_EventSourcing_EventStorage;

[Parameter(LogConsistencyProvider.ParameterName, LogConsistencyProvider.EventStorage)]
public class EventStorageLogConsistencyProvider : IConfigureSilo
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddEventStorageBasedLogConsistencyProviderAsDefault();    
    }
}