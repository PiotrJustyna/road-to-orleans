#!/bin/bash

# 2020-12-07 PJ:
# Code below gets local IP for dockerized applications to function and communicate correctly.
ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'`
GATEWAYPORT='3000'
DASHBOARDPORT='8080'
DATADOG_SERVICE_NAME='road-to-orleans-3b'
DATADOG_ENVIRONMENT='dev'
DATADOG_API_KEY='API_KEY_HERE'
ENVIRONMENT_VARIABLES_FILE="./ops/SiloHost/$DATADOG_ENVIRONMENT.env"

echo "ADVERTISEDIP=$ADVERTISEDIP
DATADOG_SERVICE_NAME=$DATADOG_SERVICE_NAME
DATADOG_ENVIRONMENT=$DATADOG_ENVIRONMENT
DATADOG_API_KEY=$DATADOG_API_KEY
GATEWAYPORT=$GATEWAYPORT
DASHBOARDPORT=$DASHBOARDPORT" > $ENVIRONMENT_VARIABLES_FILE

docker build -t silo-host -f ./ops/SiloHost/Dockerfile ./ &&
    docker-compose --env-file $ENVIRONMENT_VARIABLES_FILE -f ./ops/SiloHost/docker-compose.yml up