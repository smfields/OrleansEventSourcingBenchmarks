using Testcontainers.PostgreSql;

namespace Runner.Cluster.Parameters.GrainStorage;

public class PostgresGrainStorage : ClusterParameter, IGrainStorageProvider
{
    private PostgreSqlContainer? Container { get; set; }
    
    public override async ValueTask Initialize()
    {
        Container = new PostgreSqlBuilder()
            .WithResourceMapping("Cluster/Parameters/GrainStorage/Scripts/Postgres", "docker-entrypoint-initdb.d")
            .Build();
        
        await Container.StartAsync();
    }

    public override void ConfigureSilo(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddAdoNetGrainStorageAsDefault(cfg =>
        {
            cfg.Invariant = "Npgsql";
            cfg.ConnectionString = Container!.GetConnectionString();
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

    public override string ToString() => "PostgreSQL";
}