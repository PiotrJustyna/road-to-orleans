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

let gatewayPort () : Async<int> = async { return! port ("GATEWAYPORT") }

[<EntryPoint>]
let main args =
    let ipAddress =
        advertisedIpAddress () |> Async.RunSynchronously

    let gatewayPort = gatewayPort () |> Async.RunSynchronously
    printfn $"Starting Client on ${ipAddress.ToString()} on port ${gatewayPort}"

    0
