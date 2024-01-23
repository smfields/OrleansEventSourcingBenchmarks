using Runner.Configuration.Core;

namespace Runner.Configuration.Parameters.GrainStorage.BuiltIn;

[Parameter(GrainStorageProvider.ParameterName, GrainStorageProvider.Memory)]
public class MemoryGrainStorageProvider : IConfigureSilo
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorageAsDefault();
    }
}