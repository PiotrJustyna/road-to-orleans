open System
open System.Net
open System.Net.NetworkInformation
open System.Net.Sockets

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
        let siloPort =
            Environment.GetEnvironmentVariable("SILOPORT")
            |> int

        return siloPort
    }

let gatewayPort () : Async<int> =
    async {
        let gatewayPort =
            Environment.GetEnvironmentVariable("GATEWAYPORT")
            |> int

        return gatewayPort
    }

let primarySiloPort () : Async<int> =
    async {
        let primarySiloPort =
            Environment.GetEnvironmentVariable("PRIMARYPORT")
            |> int

        return primarySiloPort
    }

let dashboardPort () : Async<int> =
    async {
        let dashboardPort =
            Environment.GetEnvironmentVariable("DASHBOARDPORT")
            |> int

        return dashboardPort
    }

[<EntryPoint>]
let main args =
    //TODO Add Orleans host

    0
