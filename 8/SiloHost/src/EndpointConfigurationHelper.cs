using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Amazon.Util;
using Orleans.Configuration;

namespace SiloHost
{
    public static class EndpointConfigurationHelper
    {
         public static void Configure(EndpointOptions endpointOptions, string ecsContainerMetadataUri)
        {
            if (!string.IsNullOrEmpty(EnvironmentVariables.AdvertisedIp()))
                LocalEndpointSettings(endpointOptions);
            else
                ElasticContainerServiceEndpointSettings(endpointOptions, ecsContainerMetadataUri);
            
            endpointOptions.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, 2000);
            endpointOptions.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, 3000);
        }

        private static void ElasticContainerServiceEndpointSettings(EndpointOptions endpointOptions, string ecsContainerMetadataUri)
        {
            var responseBody = string.Empty;

            if (!string.IsNullOrEmpty(ecsContainerMetadataUri))
            {
                var response = new HttpClient().GetAsync(ecsContainerMetadataUri).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    responseBody = response.Content.ReadAsStringAsync().Result;
                }
            }

            var ecsMetadata = JsonSerializer.Deserialize<EcsMetadata>(responseBody);
            var ip = EC2InstanceMetadata.PrivateIpAddress;
            var siloPort = ecsMetadata?.Ports?.FirstOrDefault(x => x.ContainerPort == 2000)?.HostPort ?? 0;
            var gatewayPort = ecsMetadata?.Ports?.FirstOrDefault(x => x.ContainerPort == 3000)?.HostPort ?? 0;

            if (ip == default || siloPort == default || gatewayPort == default)
            {
                throw new Exception($"ECS metadata retrieval failed. Values received: Ip='{ip}', SiloPort='{siloPort}', GatewayPort='{gatewayPort}'.");
            }

            if (IPAddress.TryParse(ip, out var parsedIp))
                endpointOptions.AdvertisedIPAddress = parsedIp;

            endpointOptions.SiloPort = siloPort;
            endpointOptions.GatewayPort = gatewayPort;
        }

        private static void LocalEndpointSettings(EndpointOptions endpointOptions)
        {
            var siloPort = EnvironmentVariables.SiloPort;
            var gatewayPort = EnvironmentVariables.GatewayPort;

            endpointOptions.AdvertisedIPAddress = IPAddress.Parse(EnvironmentVariables.AdvertisedIp());
            endpointOptions.SiloPort = int.Parse(siloPort);
            endpointOptions.GatewayPort = int.Parse(gatewayPort);
        }
    }
}