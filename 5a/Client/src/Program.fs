module Client

open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

printfn "Starting Client"

HostBuilder()
    .ConfigureServices(fun (services: IServiceCollection) ->
        services.Configure<ConsoleLifetimeOptions>(fun (options: ConsoleLifetimeOptions) -> options.SuppressStatusMessages <- true) |> ignore
        ).ConfigureLogging(fun (builder: ILoggingBuilder) -> builder.AddConsole() |> ignore)
         .RunConsoleAsync() |> ignore
