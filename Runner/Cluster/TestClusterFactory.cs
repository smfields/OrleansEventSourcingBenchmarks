using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.TestingHost;
using Runner.Configuration;
using Runner.Configuration.Core;

namespace Runner.Cluster;

public static class TestClusterFactory
{
    private const string ClusterKeyName = "ClusterId";
    private static readonly Dictionary<Guid, object[]> Parameters = new();
    
    public static async Task<TestCluster> CreateTestCluster(params object[] parameters)
    {
        var clusterId = Guid.NewGuid();
        Parameters[clusterId] = parameters;

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

        return testCluster;
    }

    private class ClientConfigurator : IClientBuilderConfigurator
    {
        public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
        {
            var clusterId = configuration.GetValue<Guid>(ClusterKeyName);
            var configurationParameters = Parameters[clusterId];
            foreach (IConfigureClient configurationParameter in configurationParameters.Where(p => p is IConfigureClient))
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
            var configurationParameters = Parameters[clusterId];
            hostBuilder.UseOrleans(siloBuilder =>
            {
                foreach (IConfigureSilo configurationParameter in configurationParameters.Where(p => p is IConfigureSilo))
                {
                    configurationParameter.ConfigureSilo(siloBuilder);
                }
            });
        }
    }
}