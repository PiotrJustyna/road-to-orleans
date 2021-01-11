FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build

COPY ./8/SiloHost/SiloHost.csproj /src/SiloHost/SiloHost.csproj
COPY ./8/SiloHost/src/* /src/SiloHost/
COPY ./8/SiloHost/appsettings.json /src/SiloHost/

COPY ./8/Interfaces/Interfaces.csproj /src/Interfaces/Interfaces.csproj
COPY ./8/Interfaces/src/* /src/Interfaces/

COPY ./8/Grains/Grains.csproj /src/Grains/Grains.csproj
COPY ./8/Grains/src/* /src/Grains/

COPY ./8/Library/* /src/Library/

COPY ./8/ops/SiloHost/entrypoint.sh /SiloHost/

RUN dotnet publish --verbosity normal "/src/SiloHost/SiloHost.csproj" --configuration Release --output /SiloHost

EXPOSE 3000 8080

CMD ["dotnet", "/SiloHost/SiloHost.dll"]
ENTRYPOINT ["/SiloHost/entrypoint.sh"]