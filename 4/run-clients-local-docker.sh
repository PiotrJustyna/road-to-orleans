#!/bin/bash
export ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`
export PRIMARYSILOADDRESS=10.0.0.9:3008
dotnet run --project ./Client/Client.csproj
