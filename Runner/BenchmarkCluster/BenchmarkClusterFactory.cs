using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.TestingHost;

namespace Runner.BenchmarkCluster;

public class BenchmarkClusterFactory
{
    private const string ClusterKeyName = "ClusterId";
    private readonly TestClusterBuilder _testClusterBuilder = new();
    private static readonly Dictionary<Guid, Action<ISiloBuilder>> ConfigurationDelegates = new();
    
    public TestCluster CreateTestCluster(Action<ISiloBuilder> configureSilo)
    {
        var clusterId = Guid.NewGuid();
        ConfigurationDelegates[clusterId] = configureSilo;

        _testClusterBuilder
            .ConfigureHostConfiguration(config =>
            {
                config.AddInMemoryCollection(new []
                {
                    new KeyValuePair<string, string?>(ClusterKeyName, clusterId.ToString())
                });
            })
            .AddSiloBuilderConfigurator<HostConfigurator>();
        
        return _testClusterBuilder.Build();
    }
    
    private class HostConfigurator : IHostConfigurator
    {
        public void Configure(IHostBuilder hostBuilder)
        {
            var clusterId = hostBuilder.GetConfiguration().GetValue<Guid>(ClusterKeyName);
            var configurationDelegate = ConfigurationDelegates[clusterId];
            hostBuilder.UseOrleans(siloBuilder => configurationDelegate(siloBuilder));
        }
    }
}