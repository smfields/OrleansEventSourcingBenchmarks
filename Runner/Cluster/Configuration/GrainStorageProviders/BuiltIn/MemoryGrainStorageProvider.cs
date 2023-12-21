namespace Runner.Cluster.Configuration.GrainStorageProviders.BuiltIn;

public class MemoryGrainStorageProvider : IClusterParameter
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorageAsDefault();
    }

    public override string ToString() => "Memory";
}