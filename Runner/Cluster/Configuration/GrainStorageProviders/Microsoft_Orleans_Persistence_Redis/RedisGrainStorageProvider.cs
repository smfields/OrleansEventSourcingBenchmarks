using StackExchange.Redis;
using Testcontainers.Redis;

namespace Runner.Cluster.Configuration.GrainStorageProviders.Microsoft_Orleans_Persistence_Redis;

public class RedisGrainStorageProvider : IClusterParameter
{
    private RedisContainer? Container { get; set; }
    
    public async ValueTask Initialize()
    {
        Container = new RedisBuilder().Build();
        await Container.StartAsync();
    }

    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddRedisGrainStorageAsDefault(cfg =>
        {
            cfg.ConfigurationOptions = ConfigurationOptions.Parse(Container!.GetConnectionString());
        });
    }
    
    public async ValueTask DisposeAsync()
    {
        if (Container is not null)
        {
            await Container.StopAsync();
            await Container.DisposeAsync();
        }
    }

    public override string ToString() => "Redis";
}