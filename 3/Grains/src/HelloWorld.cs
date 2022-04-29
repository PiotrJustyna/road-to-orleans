using System;
using System.Threading.Tasks;
using Interfaces;
using Orleans;

namespace Grains
{
    public class HelloWorld : Orleans.Grain, IHelloWorld
    {
        public Task<string> SayHello(string name)
        {
            return Task.FromResult($@"Hello {name}!");
        }

        public async Task SayHelloFireAndForget(
            string name,
            GrainCancellationToken cancellationToken)
        {
            Console.WriteLine($"{DateTime.UtcNow} - call started");
            await Task.Delay(5000);
            Console.WriteLine($"{DateTime.UtcNow} - call finished");
        }
    }
}