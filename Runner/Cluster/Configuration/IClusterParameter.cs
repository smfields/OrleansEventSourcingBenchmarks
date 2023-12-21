namespace Runner.Cluster.Configuration;

public interface IClusterParameter
{
    public ValueTask Initialize() => ValueTask.CompletedTask;
    public void ConfigureSilo(ISiloBuilder siloBuilder) {}
    public void ConfigureClient(IClientBuilder clientBuilder) {}
}
