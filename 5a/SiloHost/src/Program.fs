open System
open System.Net
open System.Net.NetworkInformation
open System.Net.Sockets
open Grains
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Orleans.Configuration
open Orleans.Hosting
open Orleans.Statistics
open Orleans
open OrleansDashboard

let localIpAddress () : Async<IPAddress> =
    async {
        let networkInterfaces =
            NetworkInterface.GetAllNetworkInterfaces()
            |> Array.filter (fun ni -> ni.OperationalStatus.Equals(OperationalStatus.Up))

        let addresses =
            seq {
                for ni in networkInterfaces do
                    for unicastAddress in ni.GetIPProperties().UnicastAddresses do
                        yield unicastAddress.Address
            }

        return
            addresses
            |> Seq.filter
                (fun address ->
                    address.AddressFamily.Equals(AddressFamily.InterNetwork)
                    && not (IPAddress.IsLoopback(address)))
            |> Seq.head
    }

let advertisedIpAddress () : Async<IPAddress> =
    async {
        let parsed, environmentIp =
            Environment.GetEnvironmentVariable("ADVERTISEDIP")
            |> IPAddress.TryParse

        return!
            async {
                match parsed with
                | true -> return environmentIp
                | false -> return! localIpAddress ()
            }
    }

let siloPort () : Async<int> =
    async {
        let parsed, siloPort =
            Environment.GetEnvironmentVariable("SILOPORT")
            |> Int32.TryParse

        return
            match parsed with
            | true -> siloPort
            | false -> raise (ArgumentException("SILOPORT environment variable not set"))
    }

let gatewayPort () : Async<int> =
    async {
        let parsed, gatewayPort =
            Environment.GetEnvironmentVariable("GATEWAYPORT")
            |> Int32.TryParse

        return
            match parsed with
            | true -> gatewayPort
            | false -> raise (ArgumentException("GATEWAYPORT environment variable not set"))
    }

let primarySiloPort () : Async<int> =
    async {
        let parsed, primarySiloPort =
            Environment.GetEnvironmentVariable("PRIMARYPORT")
            |> Int32.TryParse

        return
            match parsed with
            | true -> primarySiloPort
            | false -> raise (ArgumentException("PRIMARYPORT environment variable not set"))
    }

let dashboardPort () : Async<int> =
    async {
        let parsed, dashboardPort =
            Environment.GetEnvironmentVariable("DASHBOARDPORT")
            |> Int32.TryParse

        return
            match parsed with
            | true -> dashboardPort
            | false -> raise (ArgumentException("DASHBOARDPORT environment variable not set"))
    }

[<EntryPoint>]
let main args =
    let portsAsync =
        async {
            let tasks =
                [ siloPort ()
                  gatewayPort ()
                  primarySiloPort ()
                  dashboardPort () ]

            let! results = tasks |> Async.Parallel

            let siloPort = results[0]
            let gatewayPort = results[1]
            let primarySiloPort = results[2]
            let dashboardPort = results[3]

            return siloPort, gatewayPort, primarySiloPort, dashboardPort
        }

    let ipAddress =
        advertisedIpAddress () |> Async.RunSynchronously

    let siloPort, gatewayPort, primarySiloPort, dashboardPort = portsAsync |> Async.RunSynchronously
    printfn $"IP Address: {ipAddress.ToString()}"
    printfn $"Silo Port: {siloPort}"
    printfn $"Gateway Port: {gatewayPort}"
    printfn $"Primary Silo Port: {primarySiloPort}"
    printfn $"Dashboard Port: {dashboardPort}"

    let builder = HostBuilder()
    let siloConfiguration =
        fun (siloBuilder: ISiloBuilder) ->
            siloBuilder
                .UseDevelopmentClustering(fun (options: DevelopmentClusterMembershipOptions) -> options.PrimarySiloEndpoint <- IPEndPoint(ipAddress, primarySiloPort))
                .UseLinuxEnvironmentStatistics()
                .UseDashboard(fun (options: DashboardOptions) -> options.Username <- "user")
                .UseDashboard(fun (options: DashboardOptions) -> options.Password <- "password")
                .UseDashboard(fun (options: DashboardOptions) -> options.Port <- dashboardPort)
                .Configure<EndpointOptions>(fun (options: EndpointOptions) -> options.SiloPort <- siloPort)
                .Configure<EndpointOptions>(fun (options: EndpointOptions) -> options.AdvertisedIPAddress <- ipAddress)
                .Configure<EndpointOptions>(fun (options: EndpointOptions) -> options.GatewayPort <- gatewayPort)
                .Configure<EndpointOptions>(fun (options: EndpointOptions) -> options.SiloListeningEndpoint <- IPEndPoint(IPAddress.Any, siloPort))
                .Configure<EndpointOptions>(fun (options: EndpointOptions) -> options.GatewayListeningEndpoint <- IPEndPoint(IPAddress.Any, gatewayPort))
                .Configure<ClusterOptions>(fun (options: ClusterOptions) -> options.ClusterId <- "cluster-of-silos")
                .Configure<ClusterOptions>(fun (options: ClusterOptions) -> options.ServiceId <- "hello-world-service")
                .ConfigureApplicationParts(fun applicationPartManager -> applicationPartManager.AddApplicationPart(typeof<HelloWorld>.Assembly).WithReferences() |> ignore) |> ignore

    let configureLogging (builder : ILoggingBuilder) =
        let filter (l : LogLevel) = l.Equals LogLevel.Error
        builder.AddFilter(filter).AddConsole().AddDebug() |> ignore

    builder.ConfigureLogging(configureLogging) |> ignore
    
    builder.UseOrleans(siloConfiguration).RunConsoleAsync()
    |> Async.AwaitTask
    |> Async.RunSynchronously

    0
