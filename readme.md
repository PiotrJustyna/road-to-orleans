- [road to orleans](#road-to-orleans)
  - [build & run](#build--run)
  - [monitoring](#monitoring)
  - [code](#code)
  - [further reading](#further-reading)

# road to orleans

This repository illustrates the road to orleans with practical, real-life examples. From most basic, to more advanced techniques. The code is written using .NET 5 and was tested on MacOS (Catalina 10.15.7) and, wherever docker is supported, Linux (Alpine 3.12).

## build & run

* IDE: build + run (first the cluster, then the client)
* docker (where supported): `run.sh`

## monitoring

Silo dashboards are available by default on `localhost:8080` unless configured otherwise in the code/`dockerfile`/`run.sh`.

## code

| branch | docker support | clustering | clustered silos | grains | clients |
| --- | --- | --- | --- | --- | --- |
| [1](../../tree/solution1/1/readme.md) | silo | - | 1 | 0 | 0 |
| [2](../../tree/solution2/readme.md) | - | - | 1 | 1 C# grain | 1 - console |
| [3](../../tree/solution3/readme.md) | silo, client | - | 1 | 1 C# grain | 1 - console |
| [4](../../tree/solution4/readme.md) | silo, client | in-memory | n | 1 C# grain | n - console |
| [5](../../tree/solution5/readme.md) | silo, client | in-memory | n | 1 C# grain interfacing F# library code | n - console |
| [6](../../tree/solution6/readme.md) | silo, client | in-memory | n | 1 C# grain interfacing F# library code | n - web api |

## further reading

* https://github.com/dotnet/orleans - orleans repository
* https://dotnet.github.io/orleans/ - orleans documentation
* https://www.microsoft.com/en-us/research/wp-content/uploads/2016/02/Orleans20Best20Practices.pdf - orleans best practices
* https://gitter.im/dotnet/orleans?at=5deaf4829319bb5190f24ffe - gitter
* https://www.microsoft.com/en-us/research/wp-content/uploads/2016/02/Orleans-MSR-TR-2014-41.pdf - virtual actors paper
* https://github.com/OrleansContrib - orbiting repositories (e.g. orleans dashboard)