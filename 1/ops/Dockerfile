FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build

COPY ./SiloHost/SiloHost.csproj /src/SiloHost.csproj
COPY ./SiloHost/src/* /src/
COPY ./ops/entrypoint.sh /SiloHost/

RUN dotnet publish --verbosity normal "/src/SiloHost.csproj" --configuration Release --output /SiloHost

EXPOSE 8080

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine

WORKDIR /SiloHost
COPY --from=build /SiloHost ./
EXPOSE 3000 8080

CMD ["dotnet", "SiloHost.dll"]
ENTRYPOINT ["./entrypoint.sh"]