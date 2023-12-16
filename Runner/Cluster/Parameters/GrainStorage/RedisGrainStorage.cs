using StackExchange.Redis;
using Testcontainers.Redis;

namespace Runner.Cluster.Parameters.GrainStorage;

public class RedisGrainStorage : ClusterParameter, IGrainStorageProvider
{
    private RedisContainer? Container { get; set; }
    
    public override async ValueTask Initialize()
    {
        Container = new RedisBuilder().Build();
        await Container.StartAsync();
    }

    public override void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddRedisGrainStorageAsDefault(cfg =>
        {
            cfg.ConfigurationOptions = ConfigurationOptions.Parse(Container!.GetConnectionString());
        });
    }
    
    public override async ValueTask DisposeAsync()
    {
        if (Container is not null)
        {
            await Container.StopAsync();
            await Container.DisposeAsync();
        }
    }

    public override string ToString() => "Redis";
}