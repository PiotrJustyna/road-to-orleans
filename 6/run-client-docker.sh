#!/bin/bash
port=$1
ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'`
GATEWAYPORT=3001

docker build -t client-cluster -f ./ops/Api/Dockerfile ./ &&
  docker run -d -p $port:80 -e ADVERTISEDIP=$ADVERTISEDIP  -e GATEWAYPORT=$GATEWAYPORT --rm client-cluster