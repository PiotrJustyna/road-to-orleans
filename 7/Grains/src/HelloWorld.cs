using Interfaces;
using Library;
using Orleans;
using System.Threading.Tasks;
using Microsoft.FeatureManagement;

namespace Grains
{
    public class HelloWorld : Grain, IHelloWorld
    {
        private readonly IFeatureManagerSnapshot _featureManagerSnapshot;
        public HelloWorld(IFeatureManagerSnapshot featureManagerSnapshot)
        {
            _featureManagerSnapshot = featureManagerSnapshot;
        }
        public async Task<string> SayHello(string name, GrainCancellationToken grainCancellationToken)
        {
            string result = null;

            if (!grainCancellationToken.CancellationToken.IsCancellationRequested)
            {
                if (await _featureManagerSnapshot.IsEnabledAsync("FeatureA"))
                {
                    result = await Task.FromResult(Say.hello(name) + $" Grain reference - {IdentityString}");
                }
                else
                {
                    result = "Disabled";
                }
            }

            return result;
        }
    }
}