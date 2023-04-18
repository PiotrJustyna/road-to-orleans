module Grains.GrainActivatorHostedService

open System
open System.Threading
open Microsoft.Extensions.Hosting
open Orleans

type GrainActivatorHostedService(client: IGrainFactory) =
    inherit BackgroundService()
    let _client = client

    override this.ExecuteAsync(_: CancellationToken) =
        task {
            let! _ =
                _client
                    .GetGrain<IYourReminderGrain>(Guid.NewGuid().ToString())
                    .WakeUp()                                                                

            System.Threading.Tasks.Task.CompletedTask
            |> ignore
        }