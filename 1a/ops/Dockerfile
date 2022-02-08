FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build

COPY ./SiloHost/SiloHost.fsproj /src/SiloHost/SiloHost.fsproj
COPY ./SiloHost/src/* /src/SiloHost/src/

COPY ./Grains/Grains.fsproj /src/Grains/Grains.fsproj
COPY ./Grains/src/* /src/Grains/src/

COPY ./ops/entrypoint.sh /SiloHost/

RUN dotnet publish --verbosity normal "/src/SiloHost/SiloHost.fsproj" --configuration Release --output /SiloHost

EXPOSE 8080

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /SiloHost
COPY --from=build /SiloHost ./
EXPOSE 3000 8080

CMD ["dotnet", "SiloHost.dll"]
ENTRYPOINT ["./entrypoint.sh"]