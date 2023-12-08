namespace Runner.Parameters.GrainStorage;

public class MemoryGrainStorageParameter : IGrainStorageParameter
{
    public void ConfigureSilo(ISiloBuilder builder)
    {
        builder.AddMemoryGrainStorageAsDefault();
    }

    public override string ToString()
    {
        return $"Memory";
    }
}