using Testcontainers.PostgreSql;

namespace Runner.Cluster.Configuration.GrainStorageProviders.Microsoft_Orleans_Persistence_AdoNet;

public class PostgresGrainStorageProvider : IClusterParameter
{
    private PostgreSqlContainer? Container { get; set; }
    
    public async ValueTask Initialize()
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

    public override string ToString() => "PostgreSQL";
}