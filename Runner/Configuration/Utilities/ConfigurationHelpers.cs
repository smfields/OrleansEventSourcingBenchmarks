using BenchmarkDotNet.Configs;
using Microsoft.Extensions.Configuration;

#if DEBUG
using BenchmarkDotNet.Jobs;
#endif

namespace Runner.Configuration.Utilities;

public static class ConfigurationHelpers
{
    public static IConfig GetBenchmarkConfig()
    {
#if DEBUG
        return new InProcessDebugConfig();
#endif

        return DefaultConfig.Instance
            .AddExporter(new CustomJsonExporter());
    }

    public static IConfigurationRoot LoadConfigurationFromFile()
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
    public class InProcessDebugConfig : DebugConfig
    {
        public override IEnumerable<Job> GetJobs()
        {
            yield return Job.InProcess;
        }
    }
#endif
}