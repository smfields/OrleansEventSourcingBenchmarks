namespace Runner.Parameters.GrainStorage;

public class MemoryGrainStorageProvider : IClusterParameter
{
    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorageAsDefault();
    }

    public override string ToString() => "Memory";
}