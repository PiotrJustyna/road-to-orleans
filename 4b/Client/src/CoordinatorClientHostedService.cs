using System;
using System.Threading;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Extensions.Hosting;
using Orleans;

namespace Client
{
    public class CoordinatorClientHostedService : IHostedService
    {
        private readonly IClusterClient _clusterClient;
        private readonly Random _generator;

        public CoordinatorClientHostedService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            _generator = new Random();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var coordinatorGrain = _clusterClient.GetGrain<ICoordinator>(_generator.Next(int.MaxValue));
            Console.WriteLine($"{await coordinatorGrain.RunTests()}");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}