using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Amazon.Util;
using Orleans.Configuration;
using OrleansDashboard;

namespace SiloHost
{
    public class SiloConfigurationHelper:ISiloConfigurationHelper
    {
        private IEnvironmentVariables _environmentVariables;
        public SiloConfigurationHelper(IEnvironmentVariables environmentVariables)
        {
            _environmentVariables = environmentVariables;
        }
         public void ConfigureEndpointOptions(EndpointOptions endpointOptions)
        {
            if (!string.IsNullOrEmpty(_environmentVariables.AdvertisedIp()))
                LocalEndpointSettings(endpointOptions);
            else
                ElasticContainerServiceEndpointSettings(endpointOptions, _environmentVariables.EcsContainerMetadataUri());
            
            endpointOptions.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, 2000);
            endpointOptions.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, 3000);
        }

         public void ConfigureDynamoClusterOptions(DynamoDBClusteringOptions clusteringOptions)
         {
             clusteringOptions.TableName = _environmentVariables.MembershipTable();
             clusteringOptions.Service = _environmentVariables.AwsRegion();
         }
         
         public void ConfigureClusterOptions(ClusterOptions clusterOptions)
         {
             clusterOptions.ClusterId = "cluster-of-silos";
             clusterOptions.ServiceId = "hello-world-service";
         }
         
         public void ConfigureDashboardOptions(DashboardOptions dashboardOptions)
         {
             dashboardOptions.Username = "piotr";
             dashboardOptions.Password = "orleans";
             dashboardOptions.Port = int.Parse(_environmentVariables.DashboardPort());
         }
         
        private void ElasticContainerServiceEndpointSettings(EndpointOptions endpointOptions, string ecsContainerMetadataUri)
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

        private void LocalEndpointSettings(EndpointOptions endpointOptions)
        {
            var siloPort = _environmentVariables.SiloPort();
            var gatewayPort = _environmentVariables.GatewayPort();

            endpointOptions.AdvertisedIPAddress = IPAddress.Parse(_environmentVariables.AdvertisedIp());
            endpointOptions.SiloPort = int.Parse(siloPort);
            endpointOptions.GatewayPort = int.Parse(gatewayPort);
        }
    }

    public interface ISiloConfigurationHelper
    { 
        void ConfigureEndpointOptions(EndpointOptions endpointOptions);
        void ConfigureDynamoClusterOptions(DynamoDBClusteringOptions clusteringOptions);
        void ConfigureClusterOptions(ClusterOptions clusteringOptions);
        void ConfigureDashboardOptions(DashboardOptions dashboardOptions);
    }
}