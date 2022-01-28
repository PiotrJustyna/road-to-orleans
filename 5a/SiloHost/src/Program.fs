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

let siloPort () : Async<int> = async { return! port ("SILOPORT") }

let gatewayPort () : Async<int> = async { return! port ("GATEWAYPORT") }

let primarySiloPort () : Async<int> = async { return! port ("PRIMARYPORT") }

let dashboardPort () : Async<int> = async { return! port ("DASHBOARDPORT") }

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

            let siloPort = results[ 0 ]
            let gatewayPort = results[ 1 ]
            let primarySiloPort = results[ 2 ]
            let dashboardPort = results[ 3 ]

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

    //TODO Add Orleans host


    0
