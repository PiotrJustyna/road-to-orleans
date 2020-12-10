#!/bin/bash
export ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`
export PEERPORT=2001
export PEERADDRESS=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`
export GATEWAYPORT=3001,3001,3002,3003
export SILOPORT=2001,2001,2002,2003
dotnet run --project ./SiloHost/SiloHost.csproj