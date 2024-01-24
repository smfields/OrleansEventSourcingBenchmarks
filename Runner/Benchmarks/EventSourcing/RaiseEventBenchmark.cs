using BenchmarkDotNet.Attributes;
using Bogus;
using Runner.Grains;

namespace Runner.Benchmarks.EventSourcing;

public class RaiseEventBenchmark : OrleansBenchmark
{
    [Params(1, 10, 100, 1_000, Priority = -100)]
    public int NumEvents { get; set; }
    
    [ParamsAllValues(Priority = 100)]
    public bool ConfirmEachEvent { get; set; }
    
    [ParamsAllValues(Priority = 101)]
    public bool Reentrant { get; set; }
    
    [Benchmark]
    public async ValueTask RaiseEvents()
    {
        var grain = Cluster.GrainFactory.GetGrain<IDataGrain>(
            Guid.NewGuid(),
            grainClassNamePrefix: Reentrant ? "Runner.Grains.DataGrainReentrant" : null
        );

        var updatedText = new Faker().Lorem.Paragraph(5);

        var tasks = Enumerable
            .Range(0, NumEvents)
            .Select(_ => grain.Update(updatedText, ConfirmEachEvent))
            .Select(valueTask => valueTask.AsTask());
        
        await Task.WhenAll(tasks);

        if (!ConfirmEachEvent)
        {
            await grain.ConfirmEvents();
        }
    }
}