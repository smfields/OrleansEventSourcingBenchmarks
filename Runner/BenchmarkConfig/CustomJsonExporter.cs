using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Reports;
using Runner.Configuration.Utilities;

namespace Runner.BenchmarkConfig;

public class CustomJsonExporter() : JsonExporter("-custom", indentJson: true, excludeMeasurements: true)
{
    protected override IReadOnlyDictionary<string, object> GetDataToSerialize(Summary summary)
    {
        var data = base.GetDataToSerialize(summary).ToDictionary();
        data.Add("Configuration", ConfigurationFileHelpers.LoadConfigurationFromFile().ToDictionary());
        return data;    
    }
}