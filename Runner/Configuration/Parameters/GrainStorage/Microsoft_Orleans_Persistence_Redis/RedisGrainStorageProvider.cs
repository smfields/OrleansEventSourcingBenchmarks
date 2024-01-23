using Runner.Configuration.Core;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace Runner.Configuration.Parameters.GrainStorage.Microsoft_Orleans_Persistence_Redis;

[Parameter(GrainStorageProvider.ParameterName, GrainStorageProvider.Redis)]
public class RedisGrainStorageProvider : 
    IAsyncInitializable, 
    IConfigureSilo, 
    IAsyncDisposable
{
    private RedisContainer? Container { get; set; }
    
    public async Task InitializeAsync()
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
}