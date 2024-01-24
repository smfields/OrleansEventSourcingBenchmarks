using Runner.Grains;

namespace Runner.Benchmarks.EventSourcing;

public class EventSourcing_AddCommentBenchmark : AddCommentBenchmark
{
    protected override IBlogPostGrain GetGrainReference()
    {
        return Cluster.GrainFactory.GetGrain<IBlogPostGrain>(
            Guid.NewGuid(),
            grainClassNamePrefix: Reentrant ? "Runner.Grains.EventSourcing.ReentrantJournaledBlogPostGrain" : "Runner.Grains.EventSourcing.JournaledBlogPostGrain"
        );
    }
}