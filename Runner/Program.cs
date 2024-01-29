using System.Reflection;
using BenchmarkDotNet.Running;

#if DEBUG
var benchmarkConfig = new InProcessDebugConfig();
#else
using BenchmarkDotNet.Configs;
using Runner.BenchmarkConfig;

var benchmarkConfig = DefaultConfig.Instance
    .AddExporter(new CustomJsonExporter());
#endif

BenchmarkSwitcher
    .FromAssembly(Assembly.GetExecutingAssembly())
    .Run(args, benchmarkConfig);
    