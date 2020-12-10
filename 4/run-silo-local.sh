#!/bin/bash
export ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`
export PRIMARYADDRESS=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`
export GATEWAYPORT=3003
export UIPORT=8083
export SILOPORT=2003
export PRIMARYPORT=2001

dotnet run --project ./SiloHost/SiloHost.csproj