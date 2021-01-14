#!/bin/bash
RetrieveIp(){
  ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'
}

ADVERTISEDIP=`RetrieveIp`
GATEWAYPORT=3001
SILOPORT=2001
DASHBOARDPORT=8081
MEMBERSHIPTABLE="test-orleans-table"
#Can also optionally setup a local dynamo db and point at it like so http://$ADVERTISEDIP:8042
AWSREGION="us-west-2"

docker build -t silo-host-cluster -f ./ops/SiloHost/Dockerfile ./ &&
  docker run -it -e ADVERTISEDIP=$ADVERTISEDIP  -e GATEWAYPORT=$GATEWAYPORT -e SILOPORT=$SILOPORT -e PRIMARYPORT=$PRIMARYPORT -e DASHBOARDPORT=$DASHBOARDPORT\
   -e PRIMARYADDRESS=$PRIMARYADDRESS -e MEMBERSHIPTABLE=$MEMBERSHIPTABLE -e AWSREGION=$AWSREGION  -p $GATEWAYPORT:3000 -p $SILOPORT:2000 -p $DASHBOARDPORT:$DASHBOARDPORT --rm silo-host-cluster