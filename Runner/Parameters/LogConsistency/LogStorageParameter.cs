using Runner.Parameters.GrainStorage;

namespace Runner.Parameters.LogConsistency;

public class LogStorageParameter(IGrainStorageParameter grainStorageParameter) : GrainStorageLogConsistencyParameter(grainStorageParameter)
{
    protected override string ProviderName => "LogStorage";

    public override void ConfigureSilo(ISiloBuilder builder)
    {
        base.ConfigureSilo(builder);
        builder.AddLogStorageBasedLogConsistencyProviderAsDefault();
    }
}