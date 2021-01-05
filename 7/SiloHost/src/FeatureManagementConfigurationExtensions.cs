using Microsoft.Extensions.Configuration;

namespace SiloHost
{
    public static class FeatureManagementConfigurationExtensions
    {
        public static IConfigurationBuilder AddFeatureFlagsConfiguration(this IConfigurationBuilder configuration)
        {
            configuration.Add(new DummyConfigurationSource());
            return configuration;
        }
    }
}
