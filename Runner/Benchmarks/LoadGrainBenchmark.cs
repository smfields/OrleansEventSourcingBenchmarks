using BenchmarkDotNet.Attributes;
using Bogus;
using Runner.Cluster;
using Runner.Cluster.Parameters;
using Runner.Cluster.Parameters.EventStorage;
using Runner.Cluster.Parameters.GrainStorage;
using Runner.Cluster.Parameters.LogConsistencyProviders;
using Runner.Grains;

namespace Runner.Benchmarks;

public abstract class LoadGrainBenchmark
{
    [Params(1, 100, 1_000, Priority = -100)]
    public int NumEvents { get; set; }
    
    public abstract ILogConsistencyProvider LogConsistencyProvider { get; set; }
    
    public abstract ClusterParameter StorageProvider { get; set; }

    private Guid ExistingGrainId { get; } = Guid.NewGuid();
    private BenchmarkCluster TestCluster { get; set; } = null!;
    
    [GlobalSetup]
    public async Task GlobalSetup()
    {
        TestCluster = await BenchmarkClusterFactory.CreateTestCluster(
            LogConsistencyProvider,
            StorageProvider
        );
        
        // Setup existing grain
        var updatedText = new Faker().Lorem.Paragraph(5);
        var grain = TestCluster.GrainFactory.GetGrain<IStringDataGrain>(ExistingGrainId);
        var tasks = Enumerable
            .Range(0, NumEvents)
            .Select(_ => grain.Update(updatedText, true))
            .Select(valueTask => valueTask.AsTask());
        await Task.WhenAll(tasks);
        await grain.Deactivate();
    }
    
    [Benchmark]
    public async ValueTask LoadGrainFromStorage()
    {
        var grain = TestCluster.GrainFactory.GetGrain<IStringDataGrain>(ExistingGrainId);
        await grain.Deactivate();
    }
    
    public class LoadGrainBenchmark_GrainStorageBasedProviders : LoadGrainBenchmark
    {
        [ParamsSource(nameof(LogConsistencyProviders))]
        public override ILogConsistencyProvider LogConsistencyProvider { get; set; } = null!;

        public IEnumerable<IGrainStorageBasedLogConsistencyProvider> LogConsistencyProviders() => 
        [
            new LogStorageLogConsistencyProvider(),
            new StateStorageLogConsistencyProvider()
        ];

        [ParamsSource(nameof(GrainStorageProviders))]
        public override ClusterParameter StorageProvider { get; set; } = null!;

        public IEnumerable<IGrainStorageProvider> GrainStorageProviders() =>
        [
            new MemoryGrainStorageProvider(),
            new RedisGrainStorage(),
            new PostgresGrainStorage()
        ];
    }
    
    public class LoadGrainBenchmark_EventStorageBasedProviders : LoadGrainBenchmark
    {
        [ParamsSource(nameof(LogConsistencyProviders))]
        public override ILogConsistencyProvider LogConsistencyProvider { get; set; } = null!;

        public IEnumerable<IEventStorageLogConsistencyProvider> LogConsistencyProviders() => 
        [
            new EventStorageLogConsistencyProvider()
        ];

        [ParamsSource(nameof(GrainStorageProviders))]
        public override ClusterParameter StorageProvider { get; set; } = null!;

        public IEnumerable<IEventStorageProvider> GrainStorageProviders() =>
        [
            new MemoryEventStorageProvider(),
            new EventStoreEventStorageProvider(),
            new MartenEventStorageProvider()
        ];
    }
}