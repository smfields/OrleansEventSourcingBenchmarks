namespace Runner.Configuration.Core;

[AttributeUsage(AttributeTargets.Class)]
public class ParameterAttribute(string parameterName, string value) : Attribute
{
    public string ParameterName { get; } = parameterName;
    public string Value { get; } = value;
}