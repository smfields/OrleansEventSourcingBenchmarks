namespace Runner.Cluster.Parameters.GrainStorage;

public class MemoryGrainStorageProvider : ClusterParameter, IGrainStorageProvider
{
    public override void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorageAsDefault();
    }

    public override string ToString()
    {
        return "Memory";
    }
}