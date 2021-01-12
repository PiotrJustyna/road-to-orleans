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
using System.Threading.Tasks;

namespace SiloHost
{
    class Program
    {        
        private static readonly string EcsContainerMetadataUri = EnvironmentVariables.EcsContainerMetadataUri();

        public static Task Main()
        {
            return new HostBuilder()
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder.UseLinuxEnvironmentStatistics();
                    siloBuilder.UseDashboard(dashboardOptions =>
                    {
                        dashboardOptions.Username = "piotr";
                        dashboardOptions.Password = "orleans";
                        dashboardOptions.Port = int.Parse(EnvironmentVariables.DashboardPort);
                    });
                    
                    //Register silo with dynamo cluster
                    siloBuilder.UseDynamoDBClustering(builder =>
                    {
                        //Connect to membership table in dynamo
                        builder.TableName = EnvironmentVariables.MembershipTable;
                        builder.Service = EnvironmentVariables.AwsRegion;
                    });
                    siloBuilder.Configure<ClusterOptions>(clusterOptions =>
                    {
                        clusterOptions.ClusterId = "cluster-of-silos";
                        clusterOptions.ServiceId = "hello-world-service";
                    });
                    siloBuilder.Configure<EndpointOptions>(options => EndpointConfigurationHelper.Configure(options, EcsContainerMetadataUri));
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
    }
}