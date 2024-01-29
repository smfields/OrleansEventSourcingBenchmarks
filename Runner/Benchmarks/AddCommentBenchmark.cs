using BenchmarkDotNet.Attributes;
using Bogus;
using Runner.Grains;

namespace Runner.Benchmarks;

public class AddCommentBenchmark : OrleansBenchmark
{
    [Params(5, Priority = -101)]
    public int CommentLength { get; set; }
    
    [Params(10, 1_000, Priority = -100)]
    public int NumComments { get; set; }
    
    [ParamsAllValues(Priority = 101)]
    public bool Reentrant { get; set; }

    protected readonly Faker Faker = new();

    protected virtual IBlogPostGrain GetGrainReference()
    {
        return Cluster.GrainFactory.GetGrain<IBlogPostGrain>(
            Guid.NewGuid(),
            grainClassNamePrefix: Reentrant ? "Runner.Grains.BlogPostGrain_Reentrant" : "Runner.Grains.BlogPostGrain"
        );
    }

    protected virtual async Task<IBlogPostGrain> CreateGrain()
    {
        var grain = GetGrainReference();
        await grain.Create("Content");
        return grain;
    }
    
    [Benchmark]
    public async ValueTask AddComments()
    {
        var grain = await CreateGrain();
        
        var tasks = Enumerable
            .Range(0, NumComments)
            .Select(_ => string.Join(" ", Faker.Lorem.Words(CommentLength)))
            .Select(comment => grain.AddComment(comment).AsTask());
        
        await Task.WhenAll(tasks);
    }
}