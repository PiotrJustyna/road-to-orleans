FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as build

COPY ./SiloHost/SiloHost.csproj /src/SiloHost/SiloHost.csproj
COPY ./SiloHost/src/* /src/SiloHost/src/

COPY ./Grains/Grains.fsproj /src/Grains/Grains.fsproj
COPY ./Grains/src/* /src/Grains/src/

COPY ./ops/entrypoint.sh /SiloHost/

RUN dotnet publish --verbosity normal "/src/SiloHost/SiloHost.csproj" --configuration Release --output /SiloHost


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine

WORKDIR /SiloHost
COPY --from=build /SiloHost ./
EXPOSE 3000 8080

RUN chmod +x entrypoint.sh
CMD ["dotnet", "SiloHost.dll"]
ENTRYPOINT ["./entrypoint.sh"]