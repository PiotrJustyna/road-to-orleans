FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build

COPY ./SiloHost/SiloHost.csproj /src/SiloHost.csproj
COPY ./SiloHost/src/* /src/
COPY ./ops/entrypoint.sh /SiloHost/

RUN dotnet publish --verbosity normal "/src/SiloHost.csproj" --configuration Release --output /SiloHost

EXPOSE 8080

CMD ["dotnet", "/SiloHost/SiloHost.dll"]
ENTRYPOINT ["/SiloHost/entrypoint.sh"]