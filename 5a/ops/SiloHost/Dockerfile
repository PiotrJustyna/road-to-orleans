FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build

WORKDIR /src

COPY ./SiloHost/SiloHost.fsproj /src/SiloHost/
COPY ./SiloHost/src/ /src/SiloHost/src/

COPY ./Interfaces/Interfaces.fsproj /src/Interfaces/
COPY ./Interfaces/src/ /src/Interfaces/src/

COPY ./Grains/Grains.fsproj /src/Grains/
COPY ./Grains/src/ /src/Grains/src

COPY ./Library/* /src/Library/

COPY ./ops/SiloHost/entrypoint.sh /SiloHost/

RUN dotnet publish --verbosity normal "./SiloHost/SiloHost.fsproj" --configuration Release --output /SiloHost

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /SiloHost
COPY --from=build /SiloHost ./
EXPOSE 3000 8080

CMD ["dotnet", "SiloHost.dll"]
ENTRYPOINT ["./entrypoint.sh"]