namespace Runner.Cluster.Parameters.EventStorage;

public class MemoryEventStorageProvider : ClusterParameter, IEventStorageProvider
{
    public override void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryEventStorageAsDefault();
    }

    public override string ToString()
    {
        return "Memory";
    }
}