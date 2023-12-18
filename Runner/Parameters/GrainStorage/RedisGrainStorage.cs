using StackExchange.Redis;
using Testcontainers.Redis;

namespace Runner.Parameters.GrainStorage;

public class RedisGrainStorage : IClusterParameter
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