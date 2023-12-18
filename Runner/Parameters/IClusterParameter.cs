namespace Runner.Parameters;

public interface IClusterParameter
{
    public ValueTask Initialize() => ValueTask.CompletedTask;
    public void ConfigureSilo(ISiloBuilder siloBuilder) {}
    public void ConfigureClient(IClientBuilder clientBuilder) {}
}
