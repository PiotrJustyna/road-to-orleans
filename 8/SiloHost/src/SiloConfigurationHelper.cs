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
        private const int DefaultGatewayPort = 3000;

        public static void ConfigureEndpointOptions(
            this ISiloBuilder siloBuilder,
            int gatewayPort,
            int siloPort,
            string ip)
        {
            siloBuilder.Configure<EndpointOptions>(endpointOptions =>
            {
                LocalEndpointSettings(
                    endpointOptions,
                    siloPort,
                    gatewayPort,
                    ip);
            });
        }
        
        public static void ConfigureEndpointOptions(
            this ISiloBuilder siloBuilder,
            string ecsMetadataUri)
        {
            siloBuilder.Configure<EndpointOptions>(endpointOptions =>
            {
                ElasticContainerServiceEndpointSettings(
                    endpointOptions,
                    ecsMetadataUri);
            });
        }

        public static void ConfigureDynamoDbClusteringOptions(
            this ISiloBuilder siloBuilder,
            string membershipTableName,
            string awsRegion)
        {
            siloBuilder.UseDynamoDBClustering(clusteringOptions =>
            {
                clusteringOptions.TableName = membershipTableName;
                clusteringOptions.Service = awsRegion;
            });
        }

        public static void ConfigureClusterOptions(
            this ISiloBuilder siloBuilder,
            string clusterId,
            string serviceId)
        {
            siloBuilder.Configure<ClusterOptions>(clusterOptions =>
            {
                clusterOptions.ClusterId = clusterId;
                clusterOptions.ServiceId = serviceId;
            });
        }

        public static void ConfigureDashboardOptions(this ISiloBuilder siloBuilder,
            string username,
            string password,
            int dashboardPort)
        {
            siloBuilder.UseDashboard(dashboardOptions =>
            {
                dashboardOptions.Username = username;
                dashboardOptions.Password = password;
                dashboardOptions.Port = dashboardPort;
            });
        }

        private static void ElasticContainerServiceEndpointSettings(
            EndpointOptions endpointOptions,
            string ecsContainerMetadataUri)
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
            var gatewayPort =
                ecsMetadata?.Ports?.FirstOrDefault(x => x.ContainerPort == DefaultGatewayPort)?.HostPort ?? 0;

            if (ip == default || siloPort == default || gatewayPort == default)
            {
                throw new Exception(
                    $"ECS metadata retrieval failed. Values received: Ip='{ip}', SiloPort='{siloPort}', GatewayPort='{gatewayPort}'.");
            }

            endpointOptions.AdvertisedIPAddress = IPAddress.Parse(ip);
            endpointOptions.SiloPort = siloPort;
            endpointOptions.GatewayPort = gatewayPort;
            endpointOptions.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, DefaultSiloPort);
            endpointOptions.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, DefaultGatewayPort);
        }

        private static void LocalEndpointSettings(
            EndpointOptions endpointOptions,
            int siloPort,
            int gatewayPort,
            string advertisedIp)
        {
            endpointOptions.AdvertisedIPAddress = string.IsNullOrEmpty(advertisedIp)
                ? GetLocalIpAddress()
                : IPAddress.Parse(advertisedIp);
            endpointOptions.SiloPort = siloPort;
            endpointOptions.GatewayPort = gatewayPort;
            endpointOptions.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, DefaultSiloPort);
            endpointOptions.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, DefaultGatewayPort);
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