using Runner.Configuration.Core;
using Testcontainers.PostgreSql;

namespace Runner.Configuration.Parameters.GrainStorage.Microsoft_Orleans_Persistence_AdoNet;

[Parameter(GrainStorageProvider.ParameterName, GrainStorageProvider.Postgres)]
public class PostgresGrainStorageProvider : 
    IAsyncInitializable,
    IConfigureSilo,
    IAsyncDisposable
{
    private PostgreSqlContainer? Container { get; set; }
    
    public async Task InitializeAsync()
    {
        Container = new PostgreSqlBuilder()
            .WithResourceMapping("PostgresScripts", "docker-entrypoint-initdb.d")
            .Build();
        
        await Container.StartAsync();
    }

    public void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddAdoNetGrainStorageAsDefault(cfg =>
        {
            cfg.Invariant = "Npgsql";
            cfg.ConnectionString = Container!.GetConnectionString();
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