using Runner.Parameters.GrainStorage;

namespace Runner.Parameters.LogConsistency;

public class StateStorageParameter(IGrainStorageParameter grainStorageParameter) : GrainStorageLogConsistencyParameter(grainStorageParameter)
{
    protected override string ProviderName => "StateStorage";

    public override void ConfigureSilo(ISiloBuilder builder)
    {
        base.ConfigureSilo(builder);
        builder.AddLogStorageBasedLogConsistencyProviderAsDefault();
    }
}