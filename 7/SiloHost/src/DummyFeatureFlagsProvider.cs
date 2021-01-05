using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Threading;

namespace SiloHost
{
    public class DummyFeatureFlagsProvider : ConfigurationProvider
    {
        private bool _value = false;
        private const ushort _reloadPeriod = 10;

        public DummyFeatureFlagsProvider()
        {
            ChangeToken.OnChange(
                () =>
                {
                    var cancellationTokenSource = new CancellationTokenSource(_reloadPeriod);
                    var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
                    return cancellationChangeToken;
                },
                Load);
        }

        public override void Load()
        {
            _value = !_value;
            Set(FeatureFlags.DummyFeatureA, _value.ToString());
            OnReload();
        }
    }
}
