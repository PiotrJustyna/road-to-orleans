#!/bin/bash

# 2020-12-07 PJ:
# Code below gets local IP for dockerized applications to function and communicate correctly.
ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`
PRIMARYADDRESS=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`
GATEWAYPORT=3001
UIPORT=8081
SILOPORT=2001
PRIMARYPORT=2001

docker build -t silo-host -f ./ops/SiloHost/Dockerfile ./ &&
  docker run -it -e ADVERTISEDIP=$ADVERTISEDIP  -e GATEWAYPORT=$GATEWAYPORT -e SILOPORT=$SILOPORT -e PRIMARYPORT=$PRIMARYPORT -e PRIMARYADDRESS=$PRIMARYADDRESS -p $GATEWAYPORT:3000 -p $SILOPORT:2000 -p $UIPORT:8080 --rm silo-host