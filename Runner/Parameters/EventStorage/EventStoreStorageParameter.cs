namespace Runner.Parameters.EventStorage;

public class EventStoreStorageParameter : IEventStorageParameter
{
    public void ConfigureSilo(ISiloBuilder builder)
    {
        builder.AddEventStoreEventStorageAsDefault();
    }

    public override string ToString()
    {
        return "EventStore";
    }
}