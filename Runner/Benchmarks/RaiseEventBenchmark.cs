using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using Bogus;
using Runner.Cluster;
using Runner.Cluster.Parameters.EventStorage;
using Runner.Cluster.Parameters.GrainStorage;
using Runner.Cluster.Parameters.LogConsistencyProviders;
using Runner.Grains;

namespace Runner.Benchmarks;

public abstract class RaiseEventBenchmark
{
    [Params(1, 100, 1_000, Priority = -100)]
    public int NumEvents { get; set; }
    
    // [ParamsAllValues(Priority = 100)]
    [Params(true, Priority = 100)]
    public bool ConfirmEachEvent { get; set; }
    
    // [ParamsAllValues(Priority = 101)]
    [Params(false, Priority = 101)]
    public bool Reentrant { get; set; }

    protected BenchmarkCluster TestCluster { get; set; } = null!;
    
    [Benchmark]
    public async ValueTask RaiseEvents()
    {
        var grain = TestCluster.GrainFactory.GetGrain<IStringDataGrain>(
            Guid.NewGuid(),
            grainClassNamePrefix: Reentrant ? "Runner.Grains.StringDataGrain_Reentrant" : null
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
    
    public class GrainStorageBasedProviders : RaiseEventBenchmark
    {
        [ParamsSource(nameof(LogConsistencyProviders))]
        public IGrainStorageBasedLogConsistencyProvider LogConsistencyProvider { get; set; } = null!;

        public IEnumerable<IGrainStorageBasedLogConsistencyProvider> LogConsistencyProviders() => 
        [
            new LogStorageLogConsistencyProvider(),
            new StateStorageLogConsistencyProvider()
        ];

        [ParamsSource(nameof(GrainStorageProviders))]
        public IGrainStorageProvider StorageProvider { get; set; } = null!;

        public IEnumerable<IGrainStorageProvider> GrainStorageProviders() =>
        [
            // new MemoryGrainStorageProvider(),
            // new RedisGrainStorage(),
            new PostgresGrainStorage()
        ];
        
        [GlobalSetup]
        public async Task GlobalSetup()
        {
            Debugger.Launch();

            while (!Debugger.IsAttached)
            {
                await Task.Delay(100);
            }
            
            TestCluster = await BenchmarkClusterFactory.CreateTestCluster(
                LogConsistencyProvider,
                StorageProvider
            );
        }
    }
    
    public class EventStorageBasedProviders : RaiseEventBenchmark
    {
        [ParamsSource(nameof(LogConsistencyProviders))]
        public IEventStorageLogConsistencyProvider LogConsistencyProvider { get; set; } = null!;

        public IEnumerable<IEventStorageLogConsistencyProvider> LogConsistencyProviders() => 
        [
            new EventStorageLogConsistencyProvider()
        ];

        [ParamsSource(nameof(GrainStorageProviders))]
        public IEventStorageProvider StorageProvider { get; set; } = null!;

        public IEnumerable<IEventStorageProvider> GrainStorageProviders() =>
        [
            // new MemoryEventStorageProvider(),
            new EventStoreEventStorageProvider(),
            new MartenEventStorageProvider()
        ];
        
        [GlobalSetup]
        public async Task GlobalSetup()
        {
            TestCluster = await BenchmarkClusterFactory.CreateTestCluster(
                LogConsistencyProvider,
                StorageProvider
            );
        }
    }
}