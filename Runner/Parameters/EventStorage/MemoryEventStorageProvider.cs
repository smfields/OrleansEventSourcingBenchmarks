namespace Runner.Parameters.EventStorage;

public class MemoryEventStorageProvider : IClusterParameter
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryEventStorageAsDefault();
    }

    public override string ToString() => "Memory";
}