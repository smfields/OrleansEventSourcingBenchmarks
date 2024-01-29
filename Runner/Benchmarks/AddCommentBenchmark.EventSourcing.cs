using Runner.Grains;

namespace Runner.Benchmarks;

public class AddCommentBenchmark_EventSourcing : AddCommentBenchmark
{
    protected override IBlogPostGrain GetGrainReference()
    {
        return Cluster.GrainFactory.GetGrain<IBlogPostGrain>(
            Guid.NewGuid(),
            grainClassNamePrefix: Reentrant ? "Runner.Grains.BlogPostGrain_EventSourcing_Reentrant" : "Runner.Grains.BlogPostGrain_EventSourcing"
        );
    }
}