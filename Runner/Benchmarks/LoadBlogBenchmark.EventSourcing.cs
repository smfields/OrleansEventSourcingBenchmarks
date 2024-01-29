using Runner.Grains;

namespace Runner.Benchmarks;

public class LoadBlogBenchmark_EventSourcing : LoadBlogBenchmark
{
    protected override IBlogPostGrain GetGrainReference()
    {
        return Cluster.GrainFactory.GetGrain<IBlogPostGrain>(
            ExistingGrainId,
            "Runner.Grains.BlogPostGrain_EventSourcing"
        );
    }
}