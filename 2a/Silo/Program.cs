using System.Diagnostics;
using Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Serilog;
using Configuration;
using OpenTelemetry.Metrics;

namespace Silo
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                TelemetryManager.InitializeTraceAndMeter("OpenTelemetry.Application.Trace", "OpenTelemetry.Application.Meter");
                Log.Logger = TelemetryManager.SerilogConfiguration();

                var hostBuilder = new HostBuilder()
                    .UseOrleans(silo => silo
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "OrleansBasics";
                        })
                        .ConfigureApplicationParts(parts => parts
                            .AddApplicationPart(typeof(HelloGrain).Assembly)
                            .WithReferences())
                        .ConfigureLogging(logging => logging.AddConsole())
                        .AddIncomingGrainCallFilter<IncomingGrainCallFilter>()
                    )
                    .ConfigureServices(services =>
                    {
                        var resourceBuilder = ResourceBuilder.CreateDefault().AddService("service-name");
                        services.AddOpenTelemetryTracing(telemetry => telemetry
                                .SetResourceBuilder(resourceBuilder)
                                .AddAspNetCoreInstrumentation()
                                .AddHttpClientInstrumentation()
                                // .AddOtlpExporter(options =>
                                // {
                                //     options.Endpoint = new Uri("http://localhost:3000/api/v1/traces"); })
                                .AddConsoleExporter());
                        services.AddOpenTelemetryMetrics(telemetry => telemetry
                                .SetResourceBuilder(resourceBuilder)
                                //.AddRuntimeInstrumentation()
                                .AddAspNetCoreInstrumentation()
                                // .AddOtlpExporter(options =>
                                // {
                                //     options.Endpoint = new Uri("http://localhost:3000/api/v1/meters"); })
                                .AddConsoleExporter());
                    });

                var host = hostBuilder.Build();
                await host.StartAsync();

                Log.Information("The Silo is active");
                Log.Information("Press Enter to terminate...");
                Console.ReadLine();
                
                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("{Error}", ex.Message);
                return 1;
            }
        }
    }
}