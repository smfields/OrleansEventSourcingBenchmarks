using BenchmarkDotNet.Attributes;
using Bogus;
using Runner.Grains;

namespace Runner.Benchmarks.Loading;

public class LoadGrainBenchmark : OrleansBenchmark
{
    [Params(1, 10, 100, 1_000)]
    public int NumUpdates { get; set; }
    
    [Params(10, 1_000)]
    public int UpdateSize { get; set; }
    
    private Guid ExistingGrainId { get; } = Guid.NewGuid();

    public override async Task GlobalSetup()
    {
        await base.GlobalSetup();
        
        // Setup existing grain
        var updatedText = new Faker().Lorem.Sentences(UpdateSize);
        var grain = Cluster.GrainFactory.GetGrain<IDataGrain>(ExistingGrainId);
        var tasks = Enumerable
            .Range(0, NumUpdates)
            .Select(_ => grain.Update(updatedText, true))
            .Select(valueTask => valueTask.AsTask());
        await Task.WhenAll(tasks);
        await grain.Deactivate();
    }

    [Benchmark]
    public async ValueTask LoadGrainFromStorage()
    {
        var grain = Cluster.GrainFactory.GetGrain<IDataGrain>(ExistingGrainId);
        await grain.Deactivate();
    }
}