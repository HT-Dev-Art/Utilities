using System.Collections;
using Microsoft.Extensions.Configuration;

namespace DevArt.Helpers.Extensions;

public static class EnvironmentVariableExtensions
{
    public static void AddFlatConfigurations<TConfig>(
        this IConfiguration configuration,
        IDictionary arguments) where TConfig : class
    {
        var configType = typeof(TConfig);
        var sectionName = configType.Name;

        foreach (var property in configType.GetProperties())
        {
            configuration[$"{sectionName}:{property.Name}"] = arguments[$"{sectionName}__{property.Name}"]?.ToString();
        }
    }
}
