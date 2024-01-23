using Runner.Configuration.Core;
using Runner.Configuration.Utilities;

namespace Runner.Configuration.Parameters.EventStorage.Orleans_EventSourcing_EventStorage.BuiltIn;

[Parameter(EventStorageProvider.ParameterName, EventStorageProvider.Memory)]
public class MemoryEventStorageProvider : IConfigureSilo
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryEventStorageAsDefault();
    }
}