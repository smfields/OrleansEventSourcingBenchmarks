using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Reports;

namespace Runner.Configuration.Utilities;

public class CustomJsonExporter() : JsonExporter("-custom", indentJson: true, excludeMeasurements: true)
{
    protected override IReadOnlyDictionary<string, object> GetDataToSerialize(Summary summary)
    {
        var data = base.GetDataToSerialize(summary).ToDictionary();
        data.Add("Configuration", ConfigurationHelpers.LoadConfigurationFromFile().ToDictionary());
        return data;    
    }
}