using BenchmarkDotNet.Attributes;
using Bogus;
using Runner.Cluster;
using Runner.Grains;
using Runner.Parameters;
using Runner.Parameters.EventStorage;
using Runner.Parameters.GrainStorage;
using Runner.Parameters.LogConsistencyProviders;

namespace Runner.Benchmarks;

public abstract class RaiseEventBenchmark
{
    [Params(1, 100, 1_000, Priority = -100)]
    public int NumEvents { get; set; }
    
    [ParamsAllValues(Priority = 100)]
    public bool ConfirmEachEvent { get; set; }
    
    [ParamsAllValues(Priority = 101)]
    public bool Reentrant { get; set; }
    
    public abstract IClusterParameter LogConsistencyProvider { get; set; }
    
    public abstract IClusterParameter StorageProvider { get; set; }

    private BenchmarkCluster TestCluster { get; set; } = null!;
    
    [GlobalSetup]
    public async Task GlobalSetup()
    {
        TestCluster = await BenchmarkClusterFactory.CreateTestCluster(
            LogConsistencyProvider,
            StorageProvider
        );
    }

    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        if (LogConsistencyProvider is IAsyncDisposable logConsistencyProvider)
        {
            await logConsistencyProvider.DisposeAsync();
        }

        if (StorageProvider is IAsyncDisposable storageProvider)
        {
            await storageProvider.DisposeAsync();
        }
    }
    
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
        public override IClusterParameter LogConsistencyProvider { get; set; } = null!;

        public IEnumerable<IClusterParameter> LogConsistencyProviders() => 
        [
            new LogStorageLogConsistencyProvider(),
            new StateStorageLogConsistencyProvider()
        ];

        [ParamsSource(nameof(GrainStorageProviders))]
        public override IClusterParameter StorageProvider { get; set; } = null!;

        public IEnumerable<IClusterParameter> GrainStorageProviders() =>
        [
            new MemoryGrainStorageProvider(),
            new RedisGrainStorage(),
            new PostgresGrainStorage()
        ];
    }
    
    public class EventStorageBasedProviders : RaiseEventBenchmark
    {
        [ParamsSource(nameof(LogConsistencyProviders))]
        public override IClusterParameter LogConsistencyProvider { get; set; } = null!;

        public IEnumerable<IClusterParameter> LogConsistencyProviders() => 
        [
            new EventStorageLogConsistencyProvider()
        ];

        [ParamsSource(nameof(GrainStorageProviders))]
        public override IClusterParameter StorageProvider { get; set; } = null!;

        public IEnumerable<IClusterParameter> GrainStorageProviders() =>
        [
            new MemoryEventStorageProvider(),
            new EventStoreEventStorageProvider(),
            new MartenEventStorageProvider()
        ];
    }
}