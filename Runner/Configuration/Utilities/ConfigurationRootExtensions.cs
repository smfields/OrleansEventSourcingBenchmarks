using Microsoft.Extensions.Configuration;

namespace Runner.Configuration.Utilities;

public static class ConfigurationRootExtensions
{
    public static Dictionary<string, object> ToDictionary(this IConfigurationRoot root)
    {
        Dictionary<string, object> RecurseChildren(IEnumerable<IConfigurationSection> children)
        {
            var dict = new Dictionary<string, object>();
            
            foreach (var child in children)
            {
                if (child.Value is not null)
                {
                    dict[child.Key] = child.Value;
                }
                else
                {
                    dict[child.Key] = RecurseChildren(child.GetChildren());
                }
            }

            return dict;
        }
        
        return RecurseChildren(root.GetChildren());
    }
}