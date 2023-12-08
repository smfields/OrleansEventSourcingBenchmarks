namespace Runner.Parameters.EventStorage;

public interface IEventStorageParameter
{
    public void ConfigureSilo(ISiloBuilder builder);
}
