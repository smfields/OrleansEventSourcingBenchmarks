using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using Orleans.TestingHost;
using Runner.Cluster;
using Runner.Configuration.Utilities;

namespace Runner.Benchmarks;

public abstract class OrleansBenchmark
{
    public object[] Parameters { get; set; } = null!;
    
    protected TestCluster Cluster { get; private set; } = null!;

    public OrleansBenchmark()
    {
        if (Environment.GetEnvironmentVariable("Debugging") == bool.TrueString)
        {
            Console.WriteLine("Waiting for debugger to attach...");
            while (!Debugger.IsAttached)
            {
                Thread.Sleep(100);
            }
        }
    }
    
    [GlobalSetup]
    public virtual async Task GlobalSetup()
    {
        if (Parameters is null)
        {
            var config = ConfigurationFileHelpers.LoadConfigurationFromFile();
            Parameters = ParameterHelpers.BuildParametersFromConfig(config).ToArray();
        }
        
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