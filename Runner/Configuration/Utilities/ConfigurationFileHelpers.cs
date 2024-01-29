using Microsoft.Extensions.Configuration;

namespace Runner.Configuration.Utilities;

public static class ConfigurationFileHelpers
{
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
}