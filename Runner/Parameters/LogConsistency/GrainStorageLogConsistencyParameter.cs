using Runner.Parameters.GrainStorage;

namespace Runner.Parameters.LogConsistency;

public abstract class GrainStorageLogConsistencyParameter(IGrainStorageParameter grainStorageParameter) : ILogConsistencyParameter
{
    protected abstract string ProviderName { get; }
    
    public virtual void ConfigureSilo(ISiloBuilder builder)
    {
        grainStorageParameter.ConfigureSilo(builder);
    }
    
    public override string ToString()
    {
        return $"{ProviderName}_{grainStorageParameter}";
    }
}