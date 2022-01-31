open System.Net
open OrleansConfiguration
open Interfaces
open Microsoft.Extensions.Logging
open Orleans
open Orleans.Configuration

[<EntryPoint>]
let main _args =
    let ipAddress =
        IpAddresses.advertisedIpAddress () |> Async.RunSynchronously

    let gatewayPort = Ports.gatewayPort () |> Async.RunSynchronously
    printfn $"Starting Client on ${ipAddress.ToString()} on port ${gatewayPort}"

    let client =
        ClientBuilder()
            .Configure<ClusterOptions>(fun (clusterOptions: ClusterOptions) ->
                clusterOptions.ClusterId <- "cluster-of-silos"
                clusterOptions.ServiceId <- "hello-world-service")
            .UseStaticClustering(IPEndPoint(ipAddress, gatewayPort))
            .ConfigureLogging(fun (builder: ILoggingBuilder) ->
                builder
                    .SetMinimumLevel(LogLevel.Information)
                    .AddConsole()
                |> ignore)
            .Build()

    // Connect client
    client.Connect
        (fun error ->
            task {
                if not (error = null) then
                    printfn $"Error connecting to cluster: ${error}"
                    return false
                else
                    printfn "Connected to cluster"
                    return true
            })
    |> Async.AwaitTask
    |> Async.RunSynchronously

    let grain = client.GetGrain<IHelloWorld>(0)
    grain.SayHello("Popcorn!")
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> printfn "%s"

    client.Close()
    |> Async.AwaitTask
    |> Async.RunSynchronously

    0
