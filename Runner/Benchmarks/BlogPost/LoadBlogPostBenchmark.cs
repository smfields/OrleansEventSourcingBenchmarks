using BenchmarkDotNet.Attributes;
using Bogus;

namespace Runner.Benchmarks.BlogPost;

public class LoadBlogPostBenchmark : OrleansBenchmark
{
    [Params(50, 5_000, 50_000)]
    public int ContentLength { get; set; }
    
    [Params(10, 1_000, 10_000)]
    public int NumComments { get; set; }
    
    private Guid ExistingGrainId { get; } = Guid.NewGuid();

    public override async Task GlobalSetup()
    {
        await base.GlobalSetup();
        
        // Setup existing grain
        var faker = new Faker();
        var grain = Cluster.GrainFactory.GetGrain<IBlogPostGrain>(ExistingGrainId);
        
        var content = string.Join(" ", faker.Lorem.Words(ContentLength));
        await grain.Create(content);
        
        var comments = Enumerable.Range(0, NumComments)
            .Select(_ => faker.Lorem.Sentence())
            .Select(comment => grain.AddComment(comment))
            .Select(valueTask => valueTask.AsTask());
        await Task.WhenAll(comments);
        
        await grain.Deactivate();
    }

    [Benchmark]
    public async ValueTask LoadGrainFromStorage()
    {
        var grain = Cluster.GrainFactory.GetGrain<IBlogPostGrain>(ExistingGrainId);
        await grain.Deactivate();
    }
}