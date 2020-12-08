FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build

COPY ./Client/Client.csproj /src/Client/Client.csproj
COPY ./Client/src/* /src/Client/

COPY ./Interfaces/Interfaces.csproj /src/Interfaces/Interfaces.csproj
COPY ./Interfaces/src/* /src/Interfaces/

COPY ./ops/Client/entrypoint.sh /Client/

RUN dotnet publish --verbosity normal "/src/Client/Client.csproj" --configuration Release --output /Client

CMD ["dotnet", "/Client/Client.dll"]
ENTRYPOINT ["/Client/entrypoint.sh"]