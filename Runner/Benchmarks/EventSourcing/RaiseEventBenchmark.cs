// using BenchmarkDotNet.Attributes;
// using Bogus;
// using Runner.Cluster;
// using Runner.Configuration;
// using Runner.Grains;
//
// namespace Runner.Benchmarks.RaiseEventBenchmarks;
//
// public abstract class RaiseEventBenchmark
// {
//     [Params(1, 10, 100, 1_000, Priority = -100)]
//     public int NumEvents { get; set; }
//     
//     [ParamsAllValues(Priority = 100)]
//     public bool ConfirmEachEvent { get; set; }
//     
//     [ParamsAllValues(Priority = 101)]
//     public bool Reentrant { get; set; }
//     
//     private BenchmarkCluster TestCluster { get; set; } = null!;
//     
//     [GlobalSetup]
//     public async Task GlobalSetup()
//     {
//         TestCluster = await TestClusterFactory.CreateTestCluster(GetClusterParameters().ToArray());
//     }
//
//     [GlobalCleanup]
//     public async Task GlobalCleanup()
//     {
//         await TestCluster.DisposeAsync();
//     }
//     
//     [Benchmark]
//     public async ValueTask RaiseEvents()
//     {
//         var grain = TestCluster.GrainFactory.GetGrain<IDataGrain>(
//             Guid.NewGuid(),
//             grainClassNamePrefix: Reentrant ? "Runner.Grains.DataGrainReentrant" : null
//         );
//
//         var updatedText = new Faker().Lorem.Paragraph(5);
//
//         var tasks = Enumerable
//             .Range(0, NumEvents)
//             .Select(_ => grain.Update(updatedText, ConfirmEachEvent))
//             .Select(valueTask => valueTask.AsTask());
//         
//         await Task.WhenAll(tasks);
//
//         if (!ConfirmEachEvent)
//         {
//             await grain.ConfirmEvents();
//         }
//     }
//     
//     protected abstract List<IConfigurationParameter> GetClusterParameters();
// }