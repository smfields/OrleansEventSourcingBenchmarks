using BenchmarkDotNet.Attributes;
using Orleans.TestingHost;
using Runner.BenchmarkCluster;
using Runner.Parameters.EventStorage;
using Runner.Parameters.GrainStorage;
using Runner.Parameters.LogConsistency;

namespace Runner.Benchmarks;

public class RaiseEventBenchmark
{
    [ParamsSource(nameof(LogConsistencyProviders))]
    public ILogConsistencyParameter LogConsistencyProvider { get; set; } = null!;

    public static IEnumerable<ILogConsistencyParameter> LogConsistencyProviders()
    {
        List<IGrainStorageParameter> grainStorageParameters = 
        [
            new MemoryGrainStorageParameter()
        ];

        foreach (var grainStorageParameter in grainStorageParameters)
        {
            yield return new LogStorageParameter(grainStorageParameter);
            yield return new StateStorageParameter(grainStorageParameter);
        }

        List<IEventStorageParameter> eventStorageParameters =
        [
            new MemoryEventStorageParameter(),
            new EventStoreStorageParameter()
        ];

        foreach (var eventStorageParameter in eventStorageParameters)
        {
            yield return new EventStorageParameter(eventStorageParameter);
        }
    }

    private TestCluster TestCluster { get; set; } = null!;
    
    [GlobalSetup]
    public async Task GlobalSetup()
    {
        var testClusterFactory = new BenchmarkClusterFactory();
        
        TestCluster =  testClusterFactory.CreateTestCluster(siloBuilder =>
        {
            LogConsistencyProvider.ConfigureSilo(siloBuilder);
        });
    }

    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        await TestCluster.DisposeAsync();
    }
    
    [Benchmark]
    public async ValueTask RaiseEvents()
    {
    }
}