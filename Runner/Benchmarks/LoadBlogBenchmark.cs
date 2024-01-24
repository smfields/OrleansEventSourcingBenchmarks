using BenchmarkDotNet.Attributes;
using Bogus;
using Runner.Grains;

namespace Runner.Benchmarks;

public class LoadBlogBenchmark : OrleansBenchmark
{
    [Params(50, 5_000, 50_000)]
    public int ContentLength { get; set; }
    
    [Params(10, 1_000, 10_000)]
    public int NumComments { get; set; }
    
    protected Guid ExistingGrainId { get; } = Guid.NewGuid();
    protected Faker Faker { get; } = new();

    protected virtual IBlogPostGrain GetGrainReference()
    {
        return Cluster.GrainFactory.GetGrain<IBlogPostGrain>(ExistingGrainId);
    }
    
    public override async Task GlobalSetup()
    {
        await base.GlobalSetup();
        
        // Setup existing grain
        var grain = GetGrainReference();
        
        var content = string.Join(" ", Faker.Lorem.Words(ContentLength));
        await grain.Create(content);
        
        var comments = Enumerable.Range(0, NumComments)
            .Select(_ => Faker.Lorem.Sentence())
            .Select(comment => grain.AddComment(comment))
            .Select(valueTask => valueTask.AsTask());
        await Task.WhenAll(comments);
        
        await grain.Deactivate();
    }

    [Benchmark]
    public async ValueTask LoadGrain()
    {
        var grain = Cluster.GrainFactory.GetGrain<IBlogPostGrain>(ExistingGrainId);
        await grain.Deactivate();
    }
}