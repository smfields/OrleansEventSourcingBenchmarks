namespace Runner.Cluster.Configuration.EventStorageProviders.Orleans_EventSourcing_EventStorage.BuiltIn;

public class MemoryEventStorageProvider : IClusterParameter
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryEventStorageAsDefault();
    }

    public override string ToString() => "Memory";
}