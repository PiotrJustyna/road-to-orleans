namespace OrleansConfiguration

open System
open System.Net
open System.Net.NetworkInformation
open System.Net.Sockets

module Ports =
    let port (environmentVariableKey: string) : Async<int> =
        let parsed, parsedPort =
            Environment.GetEnvironmentVariable(environmentVariableKey)
            |> Int32.TryParse

        let port =
            match parsed with
            | true -> parsedPort
            | false -> raise (ArgumentException($"${environmentVariableKey} environment variable not set"))

        async { return port }

    let gatewayPort () : Async<int> = async { return! port "GATEWAYPORT" }

    let siloPort () : Async<int> = async { return! port "SILOPORT" }

    let primarySiloPort () : Async<int> = async { return! port "PRIMARYPORT" }

    let dashboardPort () : Async<int> = async { return! port "DASHBOARDPORT" }

module IpAddresses =
    let localIpAddress () : Async<IPAddress> =
        let networkInterfaces =
            NetworkInterface.GetAllNetworkInterfaces()
            |> Array.filter (fun ni -> ni.OperationalStatus.Equals(OperationalStatus.Up))

        let addresses =
            seq {
                for ni in networkInterfaces do
                    for unicastAddress in ni.GetIPProperties().UnicastAddresses do
                        yield unicastAddress.Address
            }

        let address =
            addresses
            |> Seq.filter
                (fun address ->
                    address.AddressFamily.Equals(AddressFamily.InterNetwork)
                    && not (IPAddress.IsLoopback(address)))
            |> Seq.head

        async { return address }

    let advertisedIpAddress () : Async<IPAddress> =
        let parsed, environmentIp =
            Environment.GetEnvironmentVariable("ADVERTISEDIP")
            |> IPAddress.TryParse

        let ip =
            async {
                match parsed with
                | true -> return environmentIp
                | false -> return! localIpAddress ()
            }

        async { return! ip }
