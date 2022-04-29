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
            GrainCancellationToken gct)
        {
            Console.WriteLine($"{DateTime.UtcNow:hh:mm:ss.fff} - call started");

            try
            {
                await Task.Delay(5000, gct.CancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.UtcNow:hh:mm:ss.fff} - exception in the call: {e.Message}");
            }

            Console.WriteLine($"{DateTime.UtcNow:hh:mm:ss.fff} - call finished");
        }
    }
}