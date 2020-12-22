using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Grains;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Statistics;

namespace SiloHost
{
    class Program
    {
        public static Task Main()
        {
            var advertisedIp = Environment.GetEnvironmentVariable("ADVERTISEDIP");
            var advertisedIpAddress = advertisedIp == null ? GetLocalIpAddress() : IPAddress.Parse(advertisedIp);

            var extractedGatewayPort = Environment.GetEnvironmentVariable("GATEWAYPORT")?? throw new Exception("Gateway port cannot be null");
            var extractedSiloPort = Environment.GetEnvironmentVariable("SILOPORT")
                                    ?? throw new Exception("Silo port cannot be null");
            var extractDashboardPort = Environment.GetEnvironmentVariable("DASHBOARDPORT") ??
                                       throw new Exception("Dashboard port cannot be null");
            var extractedPrimaryPort = Environment.GetEnvironmentVariable("PRIMARYPORT") ?? throw new Exception("Primary port cannot be null");

            var primaryAddress = Environment.GetEnvironmentVariable("PRIMARYADDRESS") ?? throw new Exception("Primary address cannot be null");

            var siloPort = int.Parse(extractedSiloPort);
            var developmentPeerPort = int.Parse(extractedPrimaryPort);
            var gatewayPort = int.Parse(extractedGatewayPort);
            var dashboardPort = int.Parse(extractDashboardPort);
            var primaryIp = IPAddress.Parse(primaryAddress);

            var primarySiloEndpoint = new IPEndPoint(primaryIp, developmentPeerPort);

            var siloEndpointConfiguration = new SiloEndpointConfiguration(advertisedIpAddress, siloPort, gatewayPort);

            return new HostBuilder()
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder.UseLinuxEnvironmentStatistics();
                    siloBuilder.UseDashboard(dashboardOptions =>
                    {
                        dashboardOptions.Username = "piotr";
                        dashboardOptions.Password = "orleans";
                        dashboardOptions.Port = dashboardPort;
                    });
                    siloBuilder.UseDevelopmentClustering(primarySiloEndpoint);
                    siloBuilder.Configure<ClusterOptions>(clusterOptions =>
                    {
                        clusterOptions.ClusterId = "cluster-of-silos";
                        clusterOptions.ServiceId = "hello-world-service";
                    });
                    siloBuilder.Configure<EndpointOptions>(endpointOptions =>
                    {
                        endpointOptions.AdvertisedIPAddress = siloEndpointConfiguration.Ip;
                        endpointOptions.SiloPort = siloEndpointConfiguration.SiloPort;
                        endpointOptions.GatewayPort = siloEndpointConfiguration.GatewayPort;
                        endpointOptions.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, siloEndpointConfiguration.SiloPort);
                        endpointOptions.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, siloEndpointConfiguration.GatewayPort);
                    });
                    siloBuilder.ConfigureApplicationParts(applicationPartManager =>
                        applicationPartManager.AddApplicationPart(typeof(HelloWorld).Assembly).WithReferences());
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .RunConsoleAsync();
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