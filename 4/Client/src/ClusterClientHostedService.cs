using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;

namespace Client
{
    public class ClusterClientHostedService : IHostedService
    {
        private readonly ILogger<ClusterClientHostedService> _logger;

        public IClusterClient Client { get; }

        public ClusterClientHostedService(
            ILogger<ClusterClientHostedService> logger,
            ILoggerProvider loggerProvider)
        {
            _logger = logger;
            _logger.LogInformation("creating cluster client...");

            var gateways = ExtractGateways();

            Client = new ClientBuilder()
                .Configure<ClusterOptions>(clusterOptions =>
                {
                    clusterOptions.ClusterId = "cluster-of-silos";
                    clusterOptions.ServiceId = "hello-world-service";
                })
                .UseStaticClustering(gateways.ToArray())
                .ConfigureLogging(loggingBuilder =>
                    loggingBuilder.SetMinimumLevel(LogLevel.Information).AddProvider(loggerProvider))
                .Build();

            _logger.LogInformation("cluster client created");
        }

        private static List<IPEndPoint> ExtractGateways()
        {
            var endpoints = new List<IPEndPoint>();
            string[] gateways = new string[] { };
            if(Environment.GetEnvironmentVariable("SILOGATEWAYS") != null)
            {
                gateways = Environment.GetEnvironmentVariable("SILOGATEWAYS")?.Split(',');
            }
            foreach (var gateway in gateways)
            {
                var split = gateway.Split(':');
                if (split.Length == 2)
                {
                    var ip = IPAddress.Parse(split[0]);
                    var port = int.Parse(split[1]);
                    var endPoint = new IPEndPoint(ip, port);
                    endpoints.Add(endPoint);
                }
            }

            if (!endpoints.Any())
            {
                var endpoint = new IPEndPoint(GetLocalIpAddress(), 3000);
                endpoints.Add(endpoint);
            }
            return endpoints;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("connecting cluster client...");

            var attempt = 0;
            var maxAttempts = 100;
            var delay = TimeSpan.FromSeconds(1);

            return Client.Connect(async error =>
            {
                _logger.LogInformation("nope");

                if (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                if (++attempt < maxAttempts)
                {
                    _logger.LogWarning(
                        error,
                        "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                        attempt,
                        maxAttempts);

                    try
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return false;
                    }

                    return true;
                }

                _logger.LogError(
                    error,
                    "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                    attempt,
                    maxAttempts);

                return false;
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Client.Close();
            }
            catch (OrleansException error)
            {
                _logger.LogWarning(
                    error,
                    "Error while gracefully disconnecting from Orleans cluster. Will ignore and continue to shutdown.");
            }
        }

        private static IPAddress GetLocalIpAddress()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;

                var properties = network.GetIPProperties();
                if (properties.GatewayAddresses.Count == 0)
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily == AddressFamily.InterNetwork &&
                        !IPAddress.IsLoopback(address.Address))
                    {
                        return address.Address;
                    }
                }
            }

            return null;
        }
    }
}