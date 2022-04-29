using System;
using System.Threading;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Extensions.Hosting;
using Orleans;

namespace Client
{
    public class HelloWorldClientHostedService : IHostedService
    {
        private readonly IClusterClient _clusterClient;

        public HelloWorldClientHostedService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var helloWorldGrain = _clusterClient.GetGrain<IHelloWorld>(0);

            var example1 = false;

            if (example1)
            {
                Console.WriteLine($"{await helloWorldGrain.SayHello("Piotr")}");
            }
            else
            {
                var start = DateTime.UtcNow;

                var cts = new CancellationTokenSource(400);

                try
                {
                    await helloWorldGrain
                        .SayHelloFireAndForget("Piotr")
                        .WaitAsync(cts.Token)
                        .ConfigureAwait(false);
                    
                    // await helloWorldGrain.SayHelloFireAndForget(
                    //     "Piotr",
                    //     cts.Token);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{Delta(start, DateTime.UtcNow)} - exception in the call: {e.Message}");
                }

                Console.WriteLine($"{Delta(start, DateTime.UtcNow)} - call finished");
            }
        }

        public string Delta(
            DateTime start,
            DateTime finish)
        {
            return $"{(finish - start):s\\.fff}s";
        }
        
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}