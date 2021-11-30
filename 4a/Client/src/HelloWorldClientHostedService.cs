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
        private readonly Random _generator;

        public HelloWorldClientHostedService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            _generator = new Random();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // A random integer is generated to allow for a new hello world grain to be created per client creation.
            var helloWorldGrain = _clusterClient.GetGrain<IHelloWorld>(_generator.Next(int.MaxValue));
            Console.WriteLine($"{await helloWorldGrain.Factorial(3)}");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}