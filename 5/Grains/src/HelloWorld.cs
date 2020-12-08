using System.Threading.Tasks;
using Interfaces;
using Library;

namespace Grains
{
    public class HelloWorld : Orleans.Grain, IHelloWorld
    {
        public async Task<string> SayHello(string name)
        {
            Say.hello("dupa");
            return await Task.FromResult($@"Hello {name}!");
        }
    }
}