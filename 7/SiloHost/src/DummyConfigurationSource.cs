using Microsoft.Extensions.Configuration;

namespace SiloHost
{
    public class DummyConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DummyFeatureFlagsProvider();
        }
    }
}
