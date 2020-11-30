# road to orleans

The code is written using .NET 5.

## solution

* `dotnet new sln -n RoadToOrleans`

## silo host

This project is going to host our grains.

* `dotnet new console --language C# --name SiloHost`
* `dotnet sln add SiloHost/SiloHost.csproj`