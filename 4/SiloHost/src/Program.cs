using System;
using System.Linq;
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
            var parsedGatewayPort = GetAvailablePort(Environment.GetEnvironmentVariable("GATEWAYPORT"));
            var gatewayPort = parsedGatewayPort != 0 ? parsedGatewayPort : 3000;
            var parsedSiloPort = GetAvailablePort(Environment.GetEnvironmentVariable("SILOPORT"));
            var siloPort = parsedSiloPort != 0 ? parsedSiloPort : 2000;
            Console.WriteLine($"Gateway port:{gatewayPort}");
            Console.WriteLine($"Silo port:{siloPort}");

            var developmentPeerPort = int.TryParse(Environment.GetEnvironmentVariable("PEERPORT"), out var f) ? f : default(int?);
            IPEndPoint siloNode = null;
            
            if (developmentPeerPort != null && developmentPeerPort != siloPort)
            {
                var primaryPath = Environment.GetEnvironmentVariable("PEERADDRESS");
                var peerIp = IPAddress.Parse(primaryPath);
                siloNode = new IPEndPoint(peerIp, (int)developmentPeerPort);
            }

            var siloEndpointConfiguration = GetSiloEndpointConfiguration(advertisedIpAddress, gatewayPort);

            return new HostBuilder()
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder.UseLinuxEnvironmentStatistics();
                    siloBuilder.UseDashboard(dashboardOptions =>
                    {
                        dashboardOptions.Username = "piotr";
                        dashboardOptions.Password = "orleans";
                    });
                       
                    siloBuilder.UseDevelopmentClustering(siloNode);
                    siloBuilder.Configure<ClusterOptions>(clusterOptions =>
                    {
                        clusterOptions.ClusterId = "cluster-of-silos";
                        clusterOptions.ServiceId = "hello-world-service";
                    });
                    siloBuilder.Configure<EndpointOptions>(endpointOptions =>
                    {
                        endpointOptions.AdvertisedIPAddress = siloEndpointConfiguration.Ip;
                        endpointOptions.SiloPort = siloPort;
                        endpointOptions.GatewayPort = siloEndpointConfiguration.GatewayPort;
                        endpointOptions.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, siloPort);
                        endpointOptions.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, gatewayPort);
                    });
                    siloBuilder.ConfigureApplicationParts(applicationPartManager =>
                        applicationPartManager.AddApplicationPart(typeof(HelloWorld).Assembly).WithReferences());
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .RunConsoleAsync();
        }

        private static SiloEndpointConfiguration GetSiloEndpointConfiguration(
            IPAddress advertisedAddress,
            int gatewayPort)
        {

            return new SiloEndpointConfiguration(
                advertisedAddress,
                2000,
                gatewayPort);
        }

        private static int GetAvailablePort(string ports)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            var parsedPorts = Array.ConvertAll(ports.Split(','), s=> int.Parse(s));
            if (!tcpConnInfoArray.Any())
            {
                return parsedPorts.First();
            }
            foreach (var port in parsedPorts)
            {
                var foundPort = tcpConnInfoArray.Any(x => x.LocalEndPoint.Port != port);
                Console.WriteLine($"Port available:{foundPort}");
                if(foundPort){
                    return port;
                }
            }
            return 0;
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