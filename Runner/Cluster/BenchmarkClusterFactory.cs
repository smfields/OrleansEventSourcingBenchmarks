using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.TestingHost;
using Runner.Cluster.Parameters;

namespace Runner.Cluster;

public static class BenchmarkClusterFactory
{
    private const string ClusterKeyName = "ClusterId";
    private static readonly Dictionary<Guid, IClusterParameter[]> ConfigurationParameters = new();
    
    public static async Task<BenchmarkCluster> CreateTestCluster(params IClusterParameter[] clusterParameters)
    {
        var clusterId = Guid.NewGuid();
        ConfigurationParameters[clusterId] = clusterParameters;

        // Initialize all parameters
        await Task.WhenAll(clusterParameters.Select(x => x.Initialize().AsTask()));

        // Configure the cluster
        var testClusterBuilder = new TestClusterBuilder();
        testClusterBuilder
            .ConfigureHostConfiguration(config =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string?>(ClusterKeyName, clusterId.ToString())
                });
            })
            .AddSiloBuilderConfigurator<HostConfigurator>()
            .AddClientBuilderConfigurator<ClientConfigurator>();
        
        // Build the cluster
        var testCluster = testClusterBuilder.Build();
        await testCluster.DeployAsync();

        return new BenchmarkCluster(testCluster, clusterParameters);
    }

    private class ClientConfigurator : IClientBuilderConfigurator
    {
        public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
        {
            var clusterId = configuration.GetValue<Guid>(ClusterKeyName);
            var configurationParameters = ConfigurationParameters[clusterId];
            foreach (var configurationParameter in configurationParameters)
            {
                configurationParameter.ConfigureClient(clientBuilder);
            }
        }
    }
    
    private class HostConfigurator : IHostConfigurator
    {
        public void Configure(IHostBuilder hostBuilder)
        {
            var clusterId = hostBuilder.GetConfiguration().GetValue<Guid>(ClusterKeyName);
            var configurationParameters = ConfigurationParameters[clusterId];
            hostBuilder.UseOrleans(siloBuilder =>
            {
                foreach (var configurationParameter in configurationParameters)
                {
                    configurationParameter.ConfigureSilo(siloBuilder);
                }
            });
        }
    }
}