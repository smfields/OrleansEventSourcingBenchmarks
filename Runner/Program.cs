using System.Reflection;
using BenchmarkDotNet.Running;
using Runner.Configuration.Utilities;

BenchmarkSwitcher
    .FromAssembly(Assembly.GetExecutingAssembly())
    .Run(args, ConfigurationHelpers.GetBenchmarkConfig());