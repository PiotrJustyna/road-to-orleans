FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build

COPY ./SiloHost/SiloHost.csproj /src/SiloHost/SiloHost.csproj
COPY ./SiloHost/src/* /src/SiloHost/
COPY ./SiloHost/appsettings.json /src/SiloHost/

COPY ./Interfaces/Interfaces.csproj /src/Interfaces/Interfaces.csproj
COPY ./Interfaces/src/* /src/Interfaces/

COPY ./Grains/Grains.csproj /src/Grains/Grains.csproj
COPY ./Grains/src/* /src/Grains/

COPY ./Library/* /src/Library/

COPY ./ops/SiloHost/entrypoint.sh /SiloHost/

RUN dotnet publish --verbosity normal "/src/SiloHost/SiloHost.csproj" --configuration Release --output /SiloHost

EXPOSE 3000 8080

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine

WORKDIR /SiloHost
COPY --from=build /SiloHost ./
EXPOSE 3000 8080

CMD ["dotnet", "SiloHost.dll"]
ENTRYPOINT ["./entrypoint.sh"]
