namespace Runner.Cluster.Configuration;

public class NamedParameter(string name) : IClusterParameter
{
    public override string ToString() => name;
}