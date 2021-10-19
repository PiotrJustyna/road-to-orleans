#!/bin/bash

# 2020-12-07 PJ:
# Code below gets local IP for dockerized applications to function and communicate correctly.
ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'`
GATEWAYPORT=3000

docker build -t client -f ./ops/Client/Dockerfile ./ &&
  docker run -it -e ADVERTISEDIP=$ADVERTISEDIP -e GATEWAYPORT=$GATEWAYPORT --rm client
