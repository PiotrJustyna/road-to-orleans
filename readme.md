- [road to orleans](#road-to-orleans)
  - [1](#1)
    - [solution](#solution)
    - [silo host](#silo-host)

# road to orleans

The code is written using .NET 5.

## 1

This is the most basic setup: only one silo, local clustering, no clients.

| clustering | clustered silos | grains | clients |
| --- | --- | --- | --- |
| local | 1 | 0 | 0 |

### solution

* `dotnet new sln -n RoadToOrleans`

### silo host

This project is going to host our grains.

* `dotnet new console --language C# --name SiloHost`
* `dotnet sln add SiloHost/SiloHost.csproj`
* In the `SiloHost` project directory:
    * `dotnet add package Microsoft.Orleans.OrleansRuntime --version 3.3.0`
    * `dotnet add package Microsoft.Extensions.Hosting --version 5.0.0`
    * `dotnet add package OrleansDashboard --version 3.1.0`
    * `dotnet add package Microsoft.Orleans.OrleansTelemetryConsumers.Linux --version 3.3.0`
    * `dotnet add package Microsoft.Orleans.CodeGenerator.MSBuild --version 3.3.0`