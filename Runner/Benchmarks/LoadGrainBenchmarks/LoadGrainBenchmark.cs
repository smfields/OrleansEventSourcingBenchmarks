using BenchmarkDotNet.Attributes;
using Bogus;
using Runner.Cluster;
using Runner.Cluster.Configuration;
using Runner.Grains;

namespace Runner.Benchmarks.LoadGrainBenchmarks;

public abstract class LoadGrainBenchmark
{
    [Params(1, 10, 100, 1_000, Priority = -100)]
    public int NumEvents { get; set; }
    private Guid ExistingGrainId { get; } = Guid.NewGuid();
    private BenchmarkCluster TestCluster { get; set; } = null!;
    
    [GlobalSetup]
    public async Task GlobalSetup()
    {
        TestCluster = await BenchmarkClusterFactory.CreateTestCluster(GetClusterParameters().ToArray());
        
        // Setup existing grain
        var updatedText = new Faker().Lorem.Paragraph(5);
        var grain = TestCluster.GrainFactory.GetGrain<IDataGrain>(ExistingGrainId);
        var tasks = Enumerable
            .Range(0, NumEvents)
            .Select(_ => grain.Update(updatedText, true))
            .Select(valueTask => valueTask.AsTask());
        await Task.WhenAll(tasks);
        await grain.Deactivate();
    }
    
    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        await TestCluster.DisposeAsync();
    }
    
    [Benchmark]
    public async ValueTask LoadGrainFromStorage()
    {
        var grain = TestCluster.GrainFactory.GetGrain<IDataGrain>(ExistingGrainId);
        await grain.Deactivate();
    }
    
    protected abstract List<IClusterParameter> GetClusterParameters();
}