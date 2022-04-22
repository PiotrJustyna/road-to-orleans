using System.Threading.Tasks;
using Interfaces;

namespace Grains
{
    public class HelloWorld : Orleans.Grain, IHelloWorld
    {
        public ValueTask<string> SayHello(string name)
        {
          return new ValueTask<string> ($@"Hello {name}!");
        }
  }
}