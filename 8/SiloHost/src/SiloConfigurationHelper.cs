using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using Amazon.Util;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace SiloHost
{
    public static class SiloConfigurationHelper
    {
        private const int DefaultSiloPort = 2000;
        private const int DefaultGatewayPort = 2000;

        public static void ConfigureEndpointOptions(this ISiloBuilder siloBuilder, IEnvironmentVariables environmentVariableService)
        {
            siloBuilder.Configure<EndpointOptions>(endpointOptions =>
            {
                if (environmentVariableService.IsLocal())
                    LocalEndpointSettings(endpointOptions, environmentVariableService.SiloPort(), environmentVariableService.GatewayPort(), 
                        environmentVariableService.AdvertisedIp());
                else
                    ElasticContainerServiceEndpointSettings(endpointOptions, environmentVariableService.EcsContainerMetadataUri());

                endpointOptions.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, DefaultSiloPort);
                endpointOptions.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, DefaultGatewayPort);
            });
        }

         public static void ConfigureDynamoClusterOptions(this ISiloBuilder siloBuilder, IEnvironmentVariables environmentVariableService)
         {
            siloBuilder.Configure<DynamoDBClusteringOptions>(clusteringOptions =>
            {
                clusteringOptions.TableName = environmentVariableService.MembershipTable();
                clusteringOptions.Service = environmentVariableService.AwsRegion();
            });
         }
         
         public static void ConfigureClusterOptions(this ISiloBuilder siloBuilder)
         {
            siloBuilder.Configure<ClusterOptions>(clusterOptions =>
            {
                clusterOptions.ClusterId = "cluster-of-silos";
                clusterOptions.ServiceId = "hello-world-service";
            });
         }
         
         public static void ConfigureDashboardOptions(this ISiloBuilder siloBuilder, IEnvironmentVariables environmentVariables)
         {
            siloBuilder.UseDashboard(dashboardOptions =>
            {
                dashboardOptions.Username = "piotr";
                dashboardOptions.Password = "orleans";
                dashboardOptions.Port = environmentVariables.DashboardPort();
            });
         }
         
        private static void ElasticContainerServiceEndpointSettings(EndpointOptions endpointOptions, string ecsContainerMetadataUri)
        {
            var responseBody = string.Empty;

            if (string.IsNullOrEmpty(ecsContainerMetadataUri))
            {
                throw new Exception("ECS container metadata URI cannot be null.");
            }
            
            var response = new HttpClient().GetAsync(ecsContainerMetadataUri).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                responseBody = response.Content.ReadAsStringAsync().Result;
            }
            var ecsMetadata = JsonSerializer.Deserialize<EcsMetadata>(responseBody);
            var ip = EC2InstanceMetadata.PrivateIpAddress;
            var siloPort = ecsMetadata?.Ports?.FirstOrDefault(x => x.ContainerPort == DefaultSiloPort)?.HostPort ?? 0;
            var gatewayPort = ecsMetadata?.Ports?.FirstOrDefault(x => x.ContainerPort == DefaultGatewayPort)?.HostPort ?? 0;

            if (ip == default || siloPort == default || gatewayPort == default)
            {
                throw new Exception($"ECS metadata retrieval failed. Values received: Ip='{ip}', SiloPort='{siloPort}', GatewayPort='{gatewayPort}'.");
            }
                
            endpointOptions.AdvertisedIPAddress = IPAddress.Parse(ip);
            endpointOptions.SiloPort = siloPort;
            endpointOptions.GatewayPort = gatewayPort;
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
        
        private static void LocalEndpointSettings(EndpointOptions endpointOptions, int siloPort, int gatewayPort, string advertisedIp)
        {
            endpointOptions.AdvertisedIPAddress = string.IsNullOrEmpty(advertisedIp)?
                GetLocalIpAddress() : IPAddress.Parse(advertisedIp);
            endpointOptions.SiloPort = siloPort;
            endpointOptions.GatewayPort = gatewayPort;
        }
    }
}