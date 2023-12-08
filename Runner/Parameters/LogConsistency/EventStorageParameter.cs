using Runner.Parameters.EventStorage;

namespace Runner.Parameters.LogConsistency;

public class EventStorageParameter(IEventStorageParameter eventStorageParameter) : ILogConsistencyParameter
{
    public void ConfigureSilo(ISiloBuilder builder)
    {
        eventStorageParameter.ConfigureSilo(builder);
        builder.AddEventStorageBasedLogConsistencyProviderAsDefault();
    }

    public override string ToString()
    {
        return $"EventStorage_{eventStorageParameter}";
    }
}