using System.Threading.Tasks;
using Interfaces;
using Orleans;

namespace Grains
{
    public class HelloWorld : Grain, IHelloWorld
    {
        public async Task<string> SayHello(string name, GrainCancellationToken grainCancellationToken)
        {
            string result = null;

            if (!grainCancellationToken.CancellationToken.IsCancellationRequested)
            {
                result = $"Hello, {name} from {IdentityString}!";
            }
            else
            {
                result = $"Cancelled  from {IdentityString}.";
            }

            return await Task.FromResult(result);
        }
    }
}