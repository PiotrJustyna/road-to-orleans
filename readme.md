- [road to orleans](#road-to-orleans)
  - [build & run](#build--run)
  - [monitoring](#monitoring)
  - [code](#code)

# road to orleans

This repository illustrates the road to orleans. From most basic, to more advanced techniques. The code is written using .NET 5 and was tested on MacOS (Catalina 10.15.7) and, wherever docker is supported, Linux (Alpine 3.12).

## build & run

* IDE: build + run (first the cluster, then the client)
* docker (where supported): `run.sh`

## monitoring

Silo dashboards are available by default on `localhost:8080` unless configured otherwise in the code/`dockerfile`/`run.sh`.

## code

| solution | docker support | clustering | clustered silos | grains | clients |
| --- | --- | --- | --- | --- | --- |
| [1](1/readme.md) | silo | localhost | 1 | 0 | 0 |
| [2](2/readme.md) | - | localhost | 1 | 1 C# grain | console |
| [3](3/readme.md) | silo, client | localhost | 1 | 1 C# grain | console |
| [5](5/readme.md) | silo, client | localhost | 1 | 1 C# grain interfacing F# library code | console |
