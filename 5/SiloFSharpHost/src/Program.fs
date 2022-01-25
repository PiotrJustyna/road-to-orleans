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

let ipAddress () : Async<IPAddress> =
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

[<EntryPoint>]
let main args =
    //    let builder = WebApplication.CreateBuilder(args)
//    let app = builder.Build()
//
//    app.MapGet("/", Func<string>(fun () -> "Hello World!")) |> ignore
//
//    app.Run()
//
//    0 // Exit code
    printfn $"{ipAddress ()}"

    0
