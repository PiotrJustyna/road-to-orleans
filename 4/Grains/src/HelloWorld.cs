using System.Threading.Tasks;
using Interfaces;

namespace Grains
{
    public class HelloWorld : Orleans.Grain, IHelloWorld
    {
        public Task<string> SayHello(string name)
        {
            return Task.FromResult($@"Hello {name}!");
        }
    }
}