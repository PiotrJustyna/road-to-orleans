#!/bin/bash
RetrieveIp(){
  ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'
}

ADVERTISEDIP=`RetrieveIp`
PRIMARYADDRESS=`RetrieveIp`
GATEWAYPORT=3003
DASHBOARDPORT=8083
SILOPORT=2003
PRIMARYPORT=2001
MEMBERSHIPTABLE="test-orleans-table"
AWSREGION="us-west-2"
ISLOCAL=true

docker build -t silo-host-cluster -f ./ops/SiloHost/Dockerfile ./ &&
  docker run -d -e ADVERTISEDIP=$ADVERTISEDIP  -e GATEWAYPORT=$GATEWAYPORT -e SILOPORT=$SILOPORT -e DASHBOARDPORT=$DASHBOARDPORT\
   -e ISLOCAL=$ISLOCAL -p $GATEWAYPORT:3000 -p $SILOPORT:2000 -p $DASHBOARDPORT:$DASHBOARDPORT  -e MEMBERSHIPTABLE=$MEMBERSHIPTABLE -e AWSREGION=$AWSREGION --rm silo-host-cluster