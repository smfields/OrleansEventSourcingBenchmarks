using Orleans.TestingHost;
using Runner.Cluster.Parameters;

namespace Runner.Cluster;

public class BenchmarkCluster(TestCluster testCluster, params IClusterParameter[] clusterParameters) : IDisposable, IAsyncDisposable
{
    public IReadOnlyList<IClusterParameter> ClusterParameters { get; } = clusterParameters.AsReadOnly();
    public TestCluster TestCluster { get; } = testCluster;
    public IGrainFactory GrainFactory => TestCluster.GrainFactory;
    
    public void Dispose() => TestCluster.Dispose();
    public ValueTask DisposeAsync() => TestCluster.DisposeAsync();
}