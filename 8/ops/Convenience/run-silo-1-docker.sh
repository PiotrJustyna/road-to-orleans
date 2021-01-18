#!/bin/bash
RetrieveIp(){
  ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'
}

ADVERTISEDIP=`RetrieveIp`
GATEWAYPORT=3001
DASHBOARDPORT=8081
SILOPORT=2001
MEMBERSHIPTABLE="test-orleans-table"
AWSREGION="us-west-2"
ISLOCAL=true

docker build -t silo-host-cluster -f ./ops/SiloHost/Dockerfile ./ &&
  docker run -d -e AWS_REGION -e AWS_SECRET_ACCESS_KEY -e AWS_SESSION_TOKEN -e AWS_ACCESS_KEY_ID\ 
  -e ADVERTISEDIP=$ADVERTISEDIP  -e GATEWAYPORT=$GATEWAYPORT -e SILOPORT=$SILOPORT -e DASHBOARDPORT=$DASHBOARDPORT\
   -e ISLOCAL=$ISLOCAL -p $GATEWAYPORT:$GATEWAYPORT -p $SILOPORT:$SILOPORT -p $DASHBOARDPORT:$DASHBOARDPORT  -e MEMBERSHIPTABLE=$MEMBERSHIPTABLE -e AWSREGION=$AWSREGION --rm silo-host-cluster