using BenchmarkDotNet.Attributes;
using Orleans.TestingHost;
using Runner.Cluster;
using Runner.Configuration.Utilities;

namespace Runner.Benchmarks;

public abstract class OrleansBenchmark
{
    protected TestCluster Cluster { get; private set; } = null!;
    private object[] Parameters { get; set; } = null!;
    
    [GlobalSetup]
    public virtual async Task GlobalSetup()
    {
        var config = ConfigurationHelpers.LoadConfigurationFromFile();
        Parameters = ParameterHelpers.BuildParametersFromConfig(config).ToArray();
        Cluster = await TestClusterFactory.CreateTestCluster(parameters: Parameters);
    }
    
    [GlobalCleanup]
    public virtual async Task GlobalCleanup()
    {
        foreach (var parameter in Parameters)
        {
            switch (parameter)
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync();
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }
    }
}