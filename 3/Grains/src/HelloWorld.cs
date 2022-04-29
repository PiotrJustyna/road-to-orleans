using System;
using System.Threading;
using System.Threading.Tasks;
using Interfaces;
using Orleans;

namespace Grains
{
    public class HelloWorld : Grain, IHelloWorld
    {
        public Task<string> SayHello(string name)
        {
            return Task.FromResult($@"Hello {name}!");
        }

        public async Task SayHelloFireAndForget(
            string name)
        {
            _ = ManagedKickOff();
            
            // wait for as long as the caller needs
            // return
        }

        public async Task ManagedKickOff()
        {
            var cts = new CancellationTokenSource(20000);
            
            var start = DateTime.UtcNow;
            
            try
            {
                await Task.Delay(
                    5000,
                    cts.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{Delta(start, DateTime.UtcNow)} - exception in the call: {e.Message}");
            }
            
            Console.WriteLine($"{Delta(start, DateTime.UtcNow)} - long running job finished");
        }

        public string Delta(
            DateTime start,
            DateTime finish)
        {
            return $"{(finish - start):s\\.fff}s";
        }
    }
}