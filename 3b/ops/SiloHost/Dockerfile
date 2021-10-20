FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build

RUN mkdir /opt/datadog
ADD https://github.com/DataDog/dd-trace-dotnet/releases/download/v1.27.0/datadog-dotnet-apm-1.27.0-musl.tar.gz /opt/datadog
RUN cd /opt/datadog && gzip -d ./datadog-dotnet-apm-1.27.0-musl.tar.gz  && tar xf ./datadog-dotnet-apm-1.27.0-musl.tar
RUN cd /opt/datadog && sh createLogPath.sh

COPY ./SiloHost/SiloHost.csproj /src/SiloHost/SiloHost.csproj
COPY ./SiloHost/src/* /src/SiloHost/

COPY ./Interfaces/Interfaces.csproj /src/Interfaces/Interfaces.csproj
COPY ./Interfaces/src/* /src/Interfaces/

COPY ./Grains/Grains.csproj /src/Grains/Grains.csproj
COPY ./Grains/src/* /src/Grains/

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