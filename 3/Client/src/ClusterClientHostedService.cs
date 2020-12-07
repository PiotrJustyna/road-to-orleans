using System;
using System.Net;
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

            var siloAdvertisedIpAddress = IPAddress.Loopback;
            var primarySiloPort = 3000;
            
            Client = new ClientBuilder()
                .Configure<ClusterOptions>(clusterOptions =>
                {
                    clusterOptions.ClusterId = "this-is-not-relevant-yet";
                    clusterOptions.ServiceId = "this-is-not-relevant-yet";
                })
                .UseStaticClustering(new IPEndPoint(siloAdvertisedIpAddress, primarySiloPort))
                .ConfigureLogging(loggingBuilder =>
                    loggingBuilder.SetMinimumLevel(LogLevel.Information).AddProvider(loggerProvider))
                .Build();

            _logger.LogInformation("cluster client created");
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
    }
}