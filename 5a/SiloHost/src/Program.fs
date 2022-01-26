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
    let task = async {
        let! ipAddress = advertisedIpAddress ()
        let! siloPort = siloPort ()
        let! gatewayPort = gatewayPort ()
        let! primarySiloPort = primarySiloPort ()
        let! dashboardPort = dashboardPort ()
        
        return ipAddress, siloPort, gatewayPort, primarySiloPort, dashboardPort
    }
    
    let ipAddress, siloPort, gatewayPort, primarySiloPort, dashboardPort = task |> Async.RunSynchronously
    printfn $"IP Address: {ipAddress.ToString()}"
    printfn $"Silo Port: {siloPort}"
    printfn $"Gateway Port: {gatewayPort}"
    printfn $"Primary Silo Port: {primarySiloPort}"
    printfn $"Dashboard Port: {dashboardPort}"
    
    //TODO Add Orleans host


    0
