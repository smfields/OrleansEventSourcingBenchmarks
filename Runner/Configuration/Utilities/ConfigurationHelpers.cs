using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using Microsoft.Extensions.Configuration;

namespace Runner.Configuration.Utilities;

public static class ConfigurationHelpers
{
    public static IConfig GetBenchmarkConfig()
    {
#if DEBUG
        return new InProcessDebugConfig();
#endif
        
        return DefaultConfig.Instance
            .AddExporter(JsonExporter.Default)
            .AddColumn(new TagColumn("Parameters", _ => "Test"));
    }

    public static IConfiguration LoadConfigurationFromFile()
    {
        var filePath = Environment.GetEnvironmentVariable("PARAMETER_FILE");
        if (filePath is null)
        {
            throw new FileNotFoundException("Use the \"PARAMETER_FILE\" environment variable to specify the parameter configuration");
        }

        return new ConfigurationBuilder()
            .AddJsonFile(filePath)
            .Build();
    }
    
#if DEBUG
        public class InProcessDebugConfig : ManualConfig
        {
            public InProcessDebugConfig()
            {
                AddLogger(ConsoleLogger.Default);
                AddColumnProvider(DefaultColumnProviders.Instance);
                AddJob(Job.Dry);
                AddColumn(new TagColumn("MyCustomColumn", s => s));
            }
            
            // public override IEnumerable<Job> GetJobs()
            // {
            //     yield return Job.InProcess;
            // }
        }
#endif
}