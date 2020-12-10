#!/bin/bash
export IP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`
export SILOGATEWAYS="$IP":3001,"$IP":3002,"$IP":3003,"$IP":3004
dotnet run --project ./Client/Client.csproj