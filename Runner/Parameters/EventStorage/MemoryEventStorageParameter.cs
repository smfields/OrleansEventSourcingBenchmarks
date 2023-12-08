namespace Runner.Parameters.EventStorage;

public class MemoryEventStorageParameter : IEventStorageParameter
{
    public void ConfigureSilo(ISiloBuilder builder)
    {
        builder.AddMemoryEventStorageAsDefault();
    }

    public override string ToString()
    {
        return "Memory";
    }
}