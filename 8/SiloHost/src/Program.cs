using Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Statistics;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Util;

namespace SiloHost
{
    class Program
    {
        public static Task Main()
        {
            var advertisedIp = Environment.GetEnvironmentVariable("ADVERTISEDIP");
            var siloEndpointConfiguration = GetSiloEndpointConfiguration(advertisedIp);

            var extractDashboardPort = Environment.GetEnvironmentVariable("DASHBOARDPORT") ??
                                       throw new Exception("Dashboard port cannot be null");
            var awsRegion = Environment.GetEnvironmentVariable("AWSREGION") ??
                            throw new Exception("Aws region cannot be null");
            var membershipTable = Environment.GetEnvironmentVariable("MEMBERSHIPTABLE") ??
                                  throw new Exception("Membership table cannot be null");

            var dashboardPort = int.Parse(extractDashboardPort);

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

                    //Register silo with dynamo cluster
                    siloBuilder.UseDynamoDBClustering(builder =>
                    {
                        //Connect to membership table in dynamo
                        builder.TableName = membershipTable;
                        builder.Service = awsRegion;
                    });
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
                        endpointOptions.SiloListeningEndpoint =
                            new IPEndPoint(IPAddress.Any, siloEndpointConfiguration.SiloPort);
                        endpointOptions.GatewayListeningEndpoint =
                            new IPEndPoint(IPAddress.Any, siloEndpointConfiguration.GatewayPort);
                    });
                    siloBuilder.ConfigureApplicationParts(applicationPartManager =>
                        applicationPartManager.AddApplicationPart(typeof(HelloWorld).Assembly).WithReferences());

                    /*Registering Feature Management, to allow DI of IFeatureManagerSnapshot in HelloWorld grain.
                     Using built in Percentage filter to demonstrate a feature being on/off.*/
                    siloBuilder.ConfigureServices(serviceCollection =>
                    {
                        serviceCollection.AddFeatureManagement()
                            .AddFeatureFilter<PercentageFilter>();
                    });
                })
                .ConfigureLogging(logging => logging.AddConsole())

                //Registering a Configuration source for Feature Management.
                .ConfigureAppConfiguration(config => { config.AddJsonFile("appsettings.json"); })
                .RunConsoleAsync();
        }

        private static SiloEndpointConfiguration GetSiloEndpointConfiguration(string idAddr)
        {
            SiloEndpointConfiguration result = null;
            if (!string.IsNullOrWhiteSpace(idAddr))
            {
                return new SiloEndpointConfiguration(
                    idAddr,
                    2000,
                    3000);
            }

            IPAddress advertisedIpAddress = IPAddress.Parse(EC2InstanceMetadata.PrivateIpAddress);

            var metadataUri = Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI");

            if (metadataUri == null)
            {
                throw new Exception("ECS container metadata URI cannot be null.");
            }
            else
            {
                var httpClient = new HttpClient();

                // 2021-01-12 PJ:
                // It's ok to execute the call synchronously as it happens in a single-threaded, startup context.
                var response = httpClient.GetAsync(metadataUri).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseBody = response.Content.ReadAsStringAsync().Result;

                    var ecsMetadata = JsonSerializer.Deserialize<EcsMetadata>(responseBody);

                    if (ecsMetadata == null)
                    {
                        throw new Exception("ECS metadata cannot be deserialized from ECS response.");
                    }

                    if (ecsMetadata.Ports == null ||
                        !ecsMetadata.Ports.Any())
                    {
                        throw new Exception("ECS metadata does not contain any ports.");
                    }

                    var potentialSiloPort = ecsMetadata.Ports.FirstOrDefault(x => x.ContainerPort == 2000);

                    if (potentialSiloPort == null)
                    {
                        throw new Exception("Silo port not found in ECS metadata.");
                    }

                    int siloPort = potentialSiloPort.HostPort;

                    var potentialGatewayPort = ecsMetadata.Ports.FirstOrDefault(x => x.ContainerPort == 3000);

                    if (potentialGatewayPort == null)
                    {
                        throw new Exception("Gateway port not found in ECS metadata.");
                    }

                    int gatewayPort = potentialGatewayPort.HostPort;

                    var potentialDashboardPort = ecsMetadata.Ports.FirstOrDefault(x => x.ContainerPort == 8080);

                    if (potentialDashboardPort == null)
                    {
                        throw new Exception("Dashboard port not found in ECS metadata.");
                    }

                    int dashboardPort = potentialDashboardPort.HostPort;

                    return new SiloEndpointConfiguration(advertisedIpAddress, siloPort, gatewayPort, dashboardPort);
                }
                else
                {
                    throw new Exception("Could not retrieve ECS container metadata.");
                }
            }
        }
    }
}