﻿using System;
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
            var gatewayPort = int.Parse(Environment.GetEnvironmentVariable("GATEWAYPORT") ?? "3000");
            var siloPort = int.Parse(Environment.GetEnvironmentVariable("SILOPORT") ?? "2000");
            var primarySiloPort = int.TryParse(Environment.GetEnvironmentVariable("PRIMARYSILOPORT"), out var f) ? f : default(int?);
            IPEndPoint primary = null;
            
            if (primarySiloPort != null)
            {
                var primaryPath = Environment.GetEnvironmentVariable("PRIMARYSILOADDRESS");
                var ip = Dns.GetHostAddresses(primaryPath).First();
                primary = new IPEndPoint(ip, (int)primarySiloPort);
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
                       
                    siloBuilder.UseDevelopmentClustering(primary);
                    siloBuilder.Configure<ClusterOptions>(clusterOptions =>
                    {
                        clusterOptions.ClusterId = "this-is-not-relevant-yet";
                        clusterOptions.ServiceId = "this-is-not-relevant-yet";
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