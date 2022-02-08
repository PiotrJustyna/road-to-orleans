namespace SiloHost.Controllers

open System
open System.Threading.Tasks
open Grains
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Orleans

[<ApiController>]
[<Route("[controller]")>]
type DummyController(logger: ILogger<DummyController>, clusterClient: IClusterClient) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() : Task<string> =
        let grain = clusterClient.GetGrain<IDummyTests>(Int64.MinValue)
        let g = new GrainCancellationTokenSource()
        task {
            let! results = [|
                grain.DummyTest1 "a" g.Token
                grain.DummyTest2 "b" g.Token |] |> Task.WhenAll
            return $"{results.[0]}, {results.[1]}"
        }