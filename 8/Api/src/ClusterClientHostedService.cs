﻿using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;

namespace Api
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
    
            Client = new ClientBuilder()
                .Configure<ClusterOptions>(clusterOptions =>
                {
                    clusterOptions.ClusterId = "cluster-of-silos";
                    clusterOptions.ServiceId = "hello-world-service";
                }).UseDynamoDBClustering(builder =>
                {
                    //Connect to membership table in dynamo
                    builder.TableName = EnvironmentVariables.MembershipTable;
                    builder.Service = EnvironmentVariables.AwsRegion;
                })
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