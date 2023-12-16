namespace Runner.Cluster.Parameters;

public interface IClusterParameter : IAsyncDisposable
{
    public ValueTask Initialize();
    public void ConfigureSilo(ISiloBuilder siloBuilder);
    public void ConfigureClient(IClientBuilder clientBuilder);
}

public abstract class ClusterParameter : IClusterParameter
{
    public virtual ValueTask Initialize() => ValueTask.CompletedTask;
    public virtual void ConfigureSilo(ISiloBuilder siloBuilder) {}
    public virtual void ConfigureClient(IClientBuilder clientBuilder) {}
    public virtual ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public override string ToString()
    {
        return GetType().Name;
    }
}