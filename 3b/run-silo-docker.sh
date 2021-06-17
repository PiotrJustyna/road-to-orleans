#!/bin/bash



# 2020-12-07 PJ:
# Code below gets local IP for dockerized applications to function and communicate correctly.
ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`

echo "ADVERTISEDIP=$ADVERTISEDIP" > ./ops/SiloHost/dd.env

docker build -t silo-host -f ./ops/SiloHost/Dockerfile ./ &&
    docker-compose --env-file ./ops/SiloHost/dd.env -f ./ops/SiloHost/docker-compose.yml up