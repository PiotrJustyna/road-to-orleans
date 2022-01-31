open System.Net
open Grains
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Orleans.Configuration
open Orleans.Hosting
open Orleans.Statistics
open Orleans
open OrleansConfiguration
open OrleansDashboard

[<EntryPoint>]
let main args =
    let portsAsync =
        async {
            let tasks =
                [ Ports.siloPort ()
                  Ports.gatewayPort ()
                  Ports.primarySiloPort ()
                  Ports.dashboardPort () ]

            let! results = tasks |> Async.Parallel

            let siloPort = results[ 0 ]
            let gatewayPort = results[ 1 ]
            let primarySiloPort = results[ 2 ]
            let dashboardPort = results[ 3 ]

            return siloPort, gatewayPort, primarySiloPort, dashboardPort
        }

    let ipAddress =
        IpAddresses.advertisedIpAddress () |> Async.RunSynchronously

    let siloPort, gatewayPort, primarySiloPort, dashboardPort = portsAsync |> Async.RunSynchronously
    printfn $"IP Address: {ipAddress.ToString()}"
    printfn $"Silo Port: {siloPort}"
    printfn $"Gateway Port: {gatewayPort}"
    printfn $"Primary Silo Port: {primarySiloPort}"
    printfn $"Dashboard Port: {dashboardPort}"
    
    let configureLogging (builder : ILoggingBuilder) =
        let filter (l : LogLevel) = l.Equals LogLevel.Information
        builder.AddFilter(filter).AddConsole().AddDebug() |> ignore

    let builder = HostBuilder()
    let siloConfiguration =
        fun (siloBuilder: ISiloBuilder) ->
            siloBuilder
                .UseDevelopmentClustering(fun (options: DevelopmentClusterMembershipOptions) -> options.PrimarySiloEndpoint <- IPEndPoint(ipAddress, primarySiloPort))
                .Configure<ClusterOptions>(fun (options: ClusterOptions) ->
                    options.ClusterId <- "cluster-of-silos"
                    options.ServiceId <- "hello-world-service")
                .Configure<EndpointOptions>(fun (options: EndpointOptions) ->
                    options.SiloPort <- siloPort
                    options.AdvertisedIPAddress <- ipAddress
                    options.GatewayPort <- gatewayPort
                    options.SiloListeningEndpoint <- IPEndPoint(IPAddress.Any, siloPort)
                    options.GatewayListeningEndpoint <- IPEndPoint(IPAddress.Any, gatewayPort))
                .UseDashboard(fun (options: DashboardOptions) ->
                    options.Username <- "piotr"
                    options.Password <- "orleans"
                    options.Port <- dashboardPort)
                .UseLinuxEnvironmentStatistics()
                .ConfigureApplicationParts(fun applicationPartManager -> applicationPartManager.AddApplicationPart(typeof<HelloWorld>.Assembly).WithReferences() |> ignore)
                .ConfigureLogging(configureLogging) |> ignore
    
    builder.UseOrleans(siloConfiguration).RunConsoleAsync()
    |> Async.AwaitTask
    |> Async.RunSynchronously

    0
