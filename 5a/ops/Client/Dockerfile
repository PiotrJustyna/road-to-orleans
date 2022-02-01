FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build

WORKDIR /src

COPY ./Client/Client.fsproj /src/Client/
COPY ./Client/src/ /src/Client/src/

COPY ./Interfaces/Interfaces.fsproj /src/Interfaces/
COPY ./Interfaces/src/ /src/Interfaces/src/

COPY ./OrleansConfiguration/* /src/OrleansConfiguration/

COPY ./ops/Client/entrypoint.sh /Client/

RUN dotnet publish --verbosity normal "./Client/Client.fsproj" --configuration Release --output /Client

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine

WORKDIR /Client
COPY --from=build /Client ./

CMD ["dotnet", "Client.dll"]
ENTRYPOINT ["./entrypoint.sh"]