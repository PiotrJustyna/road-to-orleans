#!/bin/bash
RetrieveIp(){
  ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'
}

ADVERTISEDIP=`RetrieveIp`
PRIMARYADDRESS=`RetrieveIp`
GATEWAYPORT=3001
SILOPORT=2001
PRIMARYPORT=2001
DASHBOARDPORT=8081

docker build -t silo-host-cluster -f ./ops/Dockerfile ./ &&
  docker run -it -e ADVERTISEDIP=$ADVERTISEDIP  -e GATEWAYPORT=$GATEWAYPORT -e SILOPORT=$SILOPORT -e PRIMARYPORT=$PRIMARYPORT -e DASHBOARDPORT=$DASHBOARDPORT\
   -e PRIMARYADDRESS=$PRIMARYADDRESS -p $GATEWAYPORT:$GATEWAYPORT -p $SILOPORT:$SILOPORT -p $DASHBOARDPORT:$DASHBOARDPORT --rm silo-host-cluster