module Client.HelloWorldClientHostedService

open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Hosting

type Service() =
    interface IHostedService with
        member _.StartAsync(cancellationToken: CancellationToken) : Task = Task.CompletedTask
        member _.StopAsync(cancellationToken: CancellationToken) : Task = Task.CompletedTask
