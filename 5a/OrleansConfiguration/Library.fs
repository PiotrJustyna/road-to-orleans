namespace OrleansConfiguration

open System
open System.Net
open System.Net.NetworkInformation
open System.Net.Sockets

module Ports =
    let port (environmentVariableKey: string) : Async<int> =
        async {
            let parsed, port =
                Environment.GetEnvironmentVariable(environmentVariableKey)
                |> Int32.TryParse

            return
                match parsed with
                | true -> port
                | false -> raise (ArgumentException($"${environmentVariableKey} environment variable not set"))
        }

    let gatewayPort () : Async<int> = async { return! port "GATEWAYPORT" }

    let siloPort () : Async<int> = async { return! port "SILOPORT" }

    let primarySiloPort () : Async<int> = async { return! port "PRIMARYPORT" }

    let dashboardPort () : Async<int> = async { return! port "DASHBOARDPORT" }

module IpAddresses =
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
