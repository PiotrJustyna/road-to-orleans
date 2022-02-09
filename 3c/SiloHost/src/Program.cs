using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Grains;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Statistics;
using SiloHost;

var advertisedIp = Environment.GetEnvironmentVariable("ADVERTISEDIP");
var advertisedIpAddress = advertisedIp == null ? GetLocalIpAddress() : IPAddress.Parse(advertisedIp);
var gatewayPort = int.Parse(Environment.GetEnvironmentVariable("GATEWAYPORT") ?? "3000");
var siloEndpointConfiguration = GetSiloEndpointConfiguration(advertisedIpAddress, gatewayPort);

// Configure web host
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.WebHost.UseUrls("http://*:5000");

// Configure orleans
builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLinuxEnvironmentStatistics();
    siloBuilder.UseDashboard(dashboardOptions =>
    {
        dashboardOptions.Username = "piotr";
        dashboardOptions.Password = "orleans";
    });
    siloBuilder.UseLocalhostClustering();
    siloBuilder.Configure<EndpointOptions>(endpointOptions =>
    {
        endpointOptions.AdvertisedIPAddress = siloEndpointConfiguration.Ip;
        endpointOptions.SiloPort = siloEndpointConfiguration.SiloPort;
        endpointOptions.GatewayPort = siloEndpointConfiguration.GatewayPort;
        endpointOptions.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, 2000);
        endpointOptions.GatewayListeningEndpoint =
            new IPEndPoint(IPAddress.Any, siloEndpointConfiguration.GatewayPort);
    });
    siloBuilder.ConfigureApplicationParts(applicationPartManager =>
        applicationPartManager.AddApplicationPart(typeof(HelloWorld).Assembly).WithReferences());
});

// Configure logging
builder.WebHost.ConfigureLogging(logging => logging.AddConsole());

// Configure application
var app = builder.Build();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Run host
app.Run();

static SiloEndpointConfiguration GetSiloEndpointConfiguration(
    IPAddress advertisedAddress,
    int gatewayPort)
{
    return new(advertisedAddress, 2000, gatewayPort);
}

static IPAddress GetLocalIpAddress()
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
