using BenchmarkDotNet.Attributes;
using Bogus;
using Runner.Grains;

namespace Runner.Benchmarks.EventSourcing;

public class LoadGrainBenchmark : OrleansBenchmark
{
    // [Params(1, 10, 100, 1_000, Priority = -100)]
    public int NumEvents { get; set; } = 1;
    private Guid ExistingGrainId { get; } = Guid.NewGuid();

    public override async Task GlobalSetup()
    {
        await base.GlobalSetup();
        
        // Setup existing grain
        var updatedText = new Faker().Lorem.Paragraph(5);
        var grain = Cluster.GrainFactory.GetGrain<IDataGrain>(ExistingGrainId);
        var tasks = Enumerable
            .Range(0, NumEvents)
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