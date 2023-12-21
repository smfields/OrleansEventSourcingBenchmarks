using BenchmarkDotNet.Attributes;
using Runner.Cluster.Configuration;
using Runner.Cluster.Configuration.GrainStorageProviders;
using Runner.Cluster.Configuration.LogConsistencyProviders;

namespace Runner.Benchmarks.RaiseEventBenchmarks;

public class RaiseEventBenchmark_BuiltIn : RaiseEventBenchmark
{
    [ParamsSource(nameof(AllLogConsistencyProviders))]
    public IClusterParameter LogConsistencyProvider { get; set; } = null!;
    
    [ParamsSource(nameof(AllGrainStorageProviders))]
    public IClusterParameter GrainStorageProvider { get; set; } = null!;
    
    public static IEnumerable<IClusterParameter> AllLogConsistencyProviders() => LogConsistencyProviders.BuiltIn;
    public static IEnumerable<IClusterParameter> AllGrainStorageProviders() => GrainStorageProviders.All;
    
    protected override List<IClusterParameter> GetClusterParameters()
    {
        return [LogConsistencyProvider, GrainStorageProvider];
    }
}