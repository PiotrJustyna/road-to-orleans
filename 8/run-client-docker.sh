#!/bin/bash

# 2020-12-07 PJ:
# Code below gets local IP for dockerized applications to function and communicate correctly.
ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'`
GATEWAYPORT=3001
MEMBERSHIPTABLE="test-orleans-table"
#Can also optionally setup a local dynamo db and point at it like so http://$ADVERTISEDIP:8042
AWSREGION="us-west-2"

docker build -t client-cluster -f ./ops/Api/Dockerfile ./ &&
  docker run -it -p 5432:80 -e AWS_REGION -e AWS_SECRET_ACCESS_KEY -e AWS_SESSION_TOKEN -e AWS_ACCESS_KEY_ID \
   -e MEMBERSHIPTABLE=$MEMBERSHIPTABLE -e AWSREGION=$AWSREGION --rm client-cluster 