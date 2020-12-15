#!/bin/bash
RetrieveIp(){
  ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'
}

export ADVERTISEDIP=`RetrieveIp`
export PRIMARYADDRESS=`RetrieveIp`
export GATEWAYPORT=3002
export SILOPORT=2002
export PRIMARYPORT=2001
export DASHBOARDPORT=8082

dotnet run --project ./SiloHost/SiloHost.csproj