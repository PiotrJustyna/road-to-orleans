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
                var gcts = new GrainCancellationTokenSource();

                var cts = new CancellationTokenSource(1000);
                cts.Token.Register(() =>
                {
                    Console.WriteLine($"{DateTime.UtcNow} - cancellation token source cancelling...");
                    gcts.Cancel();
                    Console.WriteLine($"{DateTime.UtcNow} - cancellation token source cancelled");
                });

                Console.WriteLine($"{DateTime.UtcNow} - starting the call");

                try
                {
                    await helloWorldGrain.SayHelloFireAndForget("Piotr", gcts.Token);
                    // await Task.Delay(5000, cts.Token);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{DateTime.UtcNow} - exception in the call: {e.Message}");
                }

                Console.WriteLine($"{DateTime.UtcNow} - call finished");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}