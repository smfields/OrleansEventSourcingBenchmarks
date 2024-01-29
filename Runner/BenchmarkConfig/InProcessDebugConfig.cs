using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

#if DEBUG
public class InProcessDebugConfig : DebugConfig
{
    public override IEnumerable<Job> GetJobs()
    {
        yield return Job.InProcess;
    }
}
#endif