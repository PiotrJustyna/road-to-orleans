#!/bin/bash

# 2020-12-07 PJ:
# Code below gets local IP for dockerized applications to function and communicate correctly.
ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`
GATEWAYPORT='3000'
DD_AGENT_HOST=$ADVERTISEDIP
DD_API_KEY='API_KEY_HERE'
DD_VERSION='1.0'
DD_SERVICE='orleans3b'
DD_ENV='dev'

echo $DD_AGENT_HOST
docker build -t silo-host -f ./ops/SiloHost/Dockerfile ./ &&
    docker run -it -e ADVERTISEDIP=$ADVERTISEDIP -e GATEWAYPORT=$GATEWAYPORT -e DD_AGENT_HOST=$DD_AGENT_HOST -e DD_API_KEY=$DD_API_KEY -e DD_VERSION=$DD_VERSION -e DD_SERVICE=$DD_SERVICE -e DD_ENV=$DD_ENV -p $GATEWAYPORT:3000 -p 8080:8080 --rm silo-host