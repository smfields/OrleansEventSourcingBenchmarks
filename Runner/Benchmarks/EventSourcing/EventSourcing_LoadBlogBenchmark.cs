using Runner.Grains;

namespace Runner.Benchmarks.EventSourcing;

public class EventSourcing_LoadBlogBenchmark : LoadBlogBenchmark
{
    protected override IBlogPostGrain GetGrainReference()
    {
        return Cluster.GrainFactory.GetGrain<IBlogPostGrain>(
            ExistingGrainId,
            "Runner.Grains.EventSourcing.JournaledBlogPostGrain"
        );
    }
}