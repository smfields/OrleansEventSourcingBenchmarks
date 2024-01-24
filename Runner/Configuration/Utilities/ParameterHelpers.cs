using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Runner.Configuration.Core;

namespace Runner.Configuration.Utilities;

public static class ParameterHelpers
{
    public static IEnumerable<object> BuildParametersFromConfig(IConfigurationRoot configuration, Assembly? assembly = null)
    {
        var parameters = GetAllParameters(assembly);
        
        foreach (var section in configuration.GetChildren())
        {
            var parameterName = section.Key;
            var parameterType = section.Value ?? section.GetValue<string>("Type");
            yield return CreateParameterInstance(parameters, parameterName, parameterType!, section);
        }
    }

    private static ParameterRecord[] GetAllParameters(Assembly? assembly = null)
    {
        assembly ??= Assembly.GetExecutingAssembly();

        var parameters = assembly
            .GetTypes()
            .Where(t => t.GetCustomAttribute<ParameterAttribute>(inherit: true) is not null)
            .Select(t =>
            {
                var attribute = t.GetCustomAttribute<ParameterAttribute>(inherit: true)!;
                return new ParameterRecord(attribute.ParameterName, attribute.Value, t);
            })
            .ToArray();
        
        return parameters;
    }

    private static object CreateParameterInstance(IEnumerable<ParameterRecord> parameters, string parameterName, string parameterType, IConfigurationSection configurationSection)
    {
        var parameterClassType = parameters
            .Where(r => r.ParameterName == parameterName && r.ParameterType == parameterType)
            .Select(r => r.ClassType)
            .Single();

        // Check for constructor that takes configuration directly
        if (parameterClassType.GetConstructor([typeof(IConfigurationSection)]) is { } configConstructor)
        {
            return configConstructor.Invoke([configurationSection]);
        }
        
        // Check for typed options constructor
        var optionsConstructor = GetOptionsConstructor(parameterClassType);
        if (optionsConstructor.HasValue)
        {
            var (constructor, optionsType) = optionsConstructor.Value;
            var options = CreateOptionsInstance(configurationSection, optionsType);
            return constructor.Invoke([options]);
        }

        // Default constructor
        return Activator.CreateInstance(parameterClassType)!;
    }

    private static object? CreateOptionsInstance(IConfigurationSection configurationSection, Type optionsType)
    {
        var optionsClass = configurationSection.Get(optionsType);
        var typedOptionsWrapper = typeof(OptionsWrapper<>).MakeGenericType(optionsType);
        return  Activator.CreateInstance(typedOptionsWrapper, [optionsClass]);
    }

    private static (ConstructorInfo constructor, Type optionsType)? GetOptionsConstructor(Type parameterType)
    {
        foreach (var constructor in parameterType.GetConstructors())
        {
            var parameters = constructor.GetParameters();
            if (parameters is not [{ ParameterType.IsGenericType: true }]) 
                continue;
            
            var genericType = parameters[0].ParameterType.GetGenericTypeDefinition();
            if (genericType != typeof(IOptions<>)) 
                continue;
            
            var optionsType = parameters[0].ParameterType.GetGenericArguments()[0];
            return (constructor, optionsType);
        }
        return null;
    }
}