using BenchmarkDotNet.Attributes;
using Runner.Cluster.Configuration;
using Runner.Cluster.Configuration.EventStorageProviders;
using Runner.Cluster.Configuration.LogConsistencyProviders;

namespace Runner.Benchmarks.RaiseEventBenchmarks;

public class RaiseEventBenchmark_Orleans_EventSourcing_EventStorage : RaiseEventBenchmark
{
    [ParamsSource(nameof(AllLogConsistencyProviders))]
    public IClusterParameter LogConsistencyProvider { get; set; } = null!;
    
    [ParamsSource(nameof(AllGrainStorageProviders))]
    public IClusterParameter EventStorageProvider { get; set; } = null!;
    
    public static IEnumerable<IClusterParameter> AllLogConsistencyProviders() => LogConsistencyProviders.Orleans_EventSourcing_EventStorage;
    public static IEnumerable<IClusterParameter> AllGrainStorageProviders() => EventStorageProviders.Orleans_EventSourcing_EventStorage;
    
    protected override List<IClusterParameter> GetClusterParameters()
    {
        return [LogConsistencyProvider, EventStorageProvider];
    }
}