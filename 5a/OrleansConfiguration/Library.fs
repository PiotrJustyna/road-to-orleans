namespace OrleansConfiguration

open System

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
