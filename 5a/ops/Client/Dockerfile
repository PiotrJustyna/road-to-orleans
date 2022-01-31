FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

COPY ./Client/Client.fsproj /src/Client/Client.fsproj
COPY ./Client/src/* /src/Client/src/

COPY ./Interfaces/Interfaces.csproj /src/Interfaces/Interfaces.csproj
COPY ./Interfaces/src/* /src/Interfaces/

COPY ./OrleansConfiguration/* /src/OrleansConfiguration/

COPY ./ops/Client/entrypoint.sh /Client/

RUN dotnet publish --verbosity normal "/src/Client/Client.fsproj" --configuration Release --output /Client

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:6.0

WORKDIR /Client
COPY --from=build /Client ./

CMD ["dotnet", "Client.dll"]
ENTRYPOINT ["./entrypoint.sh"]