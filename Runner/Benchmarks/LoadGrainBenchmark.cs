using BenchmarkDotNet.Attributes;
using Bogus;
using Runner.Cluster;
using Runner.Grains;
using Runner.Parameters;
using Runner.Parameters.EventStorage;
using Runner.Parameters.GrainStorage;
using Runner.Parameters.LogConsistencyProviders;

namespace Runner.Benchmarks;

public abstract class LoadGrainBenchmark
{
    [Params(1, 100, 1_000, Priority = -100)]
    public int NumEvents { get; set; }
    
    public abstract IClusterParameter LogConsistencyProvider { get; set; }
    
    public abstract IClusterParameter StorageProvider { get; set; }

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
    
    public class GrainStorageBasedProviders : LoadGrainBenchmark
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
    
    public class EventStorageBasedProviders : LoadGrainBenchmark
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