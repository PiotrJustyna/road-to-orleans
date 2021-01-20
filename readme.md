- [road to orleans](#road-to-orleans)
  - [build & run](#build--run)
  - [monitoring](#monitoring)
  - [code](#code)
  - [further reading](#further-reading)

# road to orleans

This repository illustrates the road to orleans with practical, real-life examples as .NET solutions. From most basic, to more advanced techniques. The code is written using .NET 5 and was tested on MacOS (Catalina 10.15.7) and, wherever docker is supported, Linux (Alpine 3.12).

## build & run

* IDE: build + run (first the cluster, then the client)
* docker (where supported): `run.sh`

## monitoring

Silo dashboards are available by default on `localhost:8080` unless configured otherwise in the code/`dockerfile`/`run.sh`.

## code

| solution | description | docker support | clustering | clustered silos | grains | clients |
| --- | --- | --- | --- | --- | --- | --- |
| [solution1](1/readme.md) | One basic silo, no grains. | silo | - | 1 | 0 | 0 |
| [solution2](2/readme.md) | One basic silo, one grain, one console client. | - | - | 1 | 1 C# grain | 1 - console |
| [solution3](3/readme.md) | One basic silo, one grain, one console client, everything containerized. | silo, client | - | 1 | 1 C# grain | 1 - console |
| [solution4](4/readme.md) | First in-memory clustering example - many silos, many clients. | silo, client | in-memory | n | 1 C# grain | n - console |
| [solution5](5/readme.md) | Solution4 where the grain interfaces F# library code. Additionally, F# unit tests covering the F# library code. | silo, client | in-memory | n | 1 C# grain interfacing F# library code. | n - console |
| [solution6](6/readme.md) | Solution5 where the cluster is being called from a Web API. | silo, client | in-memory | n | 1 C# grain interfacing F# library code | n - web api |
| [solution7](7/readme.md) | Solution6 + [FeatureManagement](https://www.nuget.org/packages/Microsoft.FeatureManagement/), dependency injection in grains, unit tests for grains using [OrleansTestKit](https://www.nuget.org/packages/OrleansTestKit/). | silo, client | in-memory | n | 1 C# grain interfacing F# library code | n - web api |
| [solution8](8/readme.md) | Solution7 + Persistent Membership Table in Dynamo, CloudFormation Template| silo, client | Dynamo | n | 1 C# grain interfacing F# library code | n - web api |
## further reading

* https://github.com/dotnet/orleans - orleans repository
* https://dotnet.github.io/orleans/ - orleans documentation
* https://www.microsoft.com/en-us/research/wp-content/uploads/2016/02/Orleans20Best20Practices.pdf - orleans best practices
* https://gitter.im/dotnet/orleans?at=5deaf4829319bb5190f24ffe - gitter
* https://www.microsoft.com/en-us/research/wp-content/uploads/2016/02/Orleans-MSR-TR-2014-41.pdf - virtual actors paper
* https://github.com/OrleansContrib - orbiting repositories (e.g. orleans dashboard)
* https://github.com/PiotrJustyna/FeatureManagementSandbox - more involved use cases for [FeatureManagement](https://www.nuget.org/packages/Microsoft.FeatureManagement/)