using System.Threading.Tasks;
using Interfaces;

namespace Grains
{
    public class HelloWorld : Orleans.Grain, IHelloWorld
    {
        public async Task<string> SayHello(string name)
        {
            return await Task.FromResult($@"Hello {name}!");
        }
    }
}