using System.Threading.Tasks;
using Interfaces;
using Library;

namespace Grains
{
    public class HelloWorld : Orleans.Grain, IHelloWorld
    {
        public Task<string> SayHello(string name)
        {
            return Task.FromResult(Say.hello(name));
        }
    }
}