using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Runner.BenchmarkConfig;

var benchmarkConfig = DefaultConfig.Instance
    .AddExporter(new CustomJsonExporter())
    .AddJob(Job.Default.WithEnvironmentVariable("Debugging", Debugger.IsAttached.ToString()));

BenchmarkSwitcher
    .FromAssembly(Assembly.GetExecutingAssembly())
    .Run(args, benchmarkConfig);
    