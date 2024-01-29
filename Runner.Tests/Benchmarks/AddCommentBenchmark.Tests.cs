using Runner.Benchmarks;

namespace Runner.Tests.Benchmarks;

public class AddCommentBenchmark_Tests
{
    private AddCommentBenchmark Benchmark { get; set; } = null!;
    
    [SetUp]
    public async Task BenchmarkSetup()
    {
        Benchmark = new AddCommentBenchmark
        {
            Parameters = []
        };
        await Benchmark.GlobalSetup();
    }

    [TearDown]
    public async Task BenchmarkTeardown()
    {
        await Benchmark.GlobalCleanup();
    }
    
    [Test]
    public void AddComments_runs_without_exception()
    {
        Assert.DoesNotThrowAsync(async () => await Benchmark.AddComments());
    }
}