open System
open System.Net
open System.Reflection
open Interfaces
open OrleansConfiguration
open Microsoft.Extensions.Logging
open Orleans
open Orleans.Configuration
open Orleans.Hosting

[<EntryPoint>]
let main _args =
    let ipAddress =
        IpAddresses.advertisedIpAddress () |> Async.RunSynchronously

    let gatewayPort = Ports.gatewayPort () |> Async.RunSynchronously
    printfn $"Starting Client on ${ipAddress.ToString()} on port ${gatewayPort}"
    
    let mutable attempts = 0
    let maxAttempts = 100

    let client =
        ClientBuilder()
            .Configure<ClusterOptions>(fun (clusterOptions: ClusterOptions) ->
                clusterOptions.ClusterId <- "cluster-of-silos"
                clusterOptions.ServiceId <- "hello-world-service")
            .UseStaticClustering(IPEndPoint(ipAddress, gatewayPort))
            .ConfigureApplicationParts(fun applicationPartManager -> applicationPartManager.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences().WithCodeGeneration() |> ignore)
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
                do! Async.Sleep(TimeSpan.FromSeconds(1))
                attempts <- attempts + 1
                printfn $"Failed to connect to cluster on attempt {attempts} of {maxAttempts}"

                return if attempts > maxAttempts then false
                    else not(error = null)
            })
    |> Async.AwaitTask
    |> Async.RunSynchronously

    // Generate random grain number key
    let key = (Random().Next() % 100)
    
    let grain = client.GetGrain<IHelloWorld>(key)
    grain.SayHello("Popcorn!")
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> printfn "%s"

    client.Close()
    |> Async.AwaitTask
    |> Async.RunSynchronously

    0
