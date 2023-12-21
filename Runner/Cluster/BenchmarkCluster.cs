using Orleans.TestingHost;
using Runner.Cluster.Configuration;

namespace Runner.Cluster;

public class BenchmarkCluster(TestCluster testCluster, params IClusterParameter[] clusterParameters) : IAsyncDisposable
{
    public IReadOnlyList<IClusterParameter> ClusterParameters { get; } = clusterParameters.AsReadOnly();
    public TestCluster TestCluster { get; } = testCluster;
    public IGrainFactory GrainFactory => TestCluster.GrainFactory;

    public async ValueTask DisposeAsync()
    {
        await TestCluster.DisposeAsync();

        foreach (var clusterParameter in ClusterParameters)
        {
            if (clusterParameter is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
        }
    }
}