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
using Amazon.Util.Internal;

namespace SiloHost
{
    class Program
    {
        private static readonly EnvironmentVariables environmentVariable = new EnvironmentVariables();
        private readonly string EcsContainerMetadataUri = environmentVariable.EcsContainerMetadataUri();

        public static Task Main()
        {
            ISiloConfigurationHelper siloConfigurationHelper = new SiloConfigurationHelper(environmentVariable);

            return new HostBuilder()
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder.UseLinuxEnvironmentStatistics();
                    siloBuilder.UseDashboard(dashboardOptions => siloConfigurationHelper.ConfigureDashboardOptions(dashboardOptions));
                    //Register silo with dynamo cluster
                    siloBuilder.UseDynamoDBClustering(builder => siloConfigurationHelper.ConfigureDynamoClusterOptions(builder));
                    siloBuilder.Configure<ClusterOptions>(clusterOptions => siloConfigurationHelper.ConfigureClusterOptions(clusterOptions));
                    siloBuilder.Configure<EndpointOptions>(options => siloConfigurationHelper.ConfigureEndpointOptions(options));
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