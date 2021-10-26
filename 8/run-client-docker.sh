#!/bin/bash

# 2020-12-07 PJ:
# Code below gets local IP for dockerized applications to function and communicate correctly.
ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'`
GATEWAYPORT=3001
OrleansSettings__MembershipTable="some-table-name"
OrleansSettings__AwsRegion="us-west-2"

AWS_REGION=$(awk -F '=' '/^region/ { print $2 }' ~/.aws/credentials)
AWS_SECRET_ACCESS_KEY=$(awk -F '=' '/^aws_secret_access_key/ { print $2 }' ~/.aws/credentials)
AWS_SESSION_TOKEN=$(awk -F '=' '/^aws_session_token/ { print $2 }' ~/.aws/credentials)
AWS_ACCESS_KEY_ID=$(awk -F '=' '/^aws_access_key_id/ { print $2 }' ~/.aws/credentials)

docker build -t client-cluster -f ./ops/Api/Dockerfile ./ 
  docker run -it -p 5432:80 -e AWS_REGION -e AWS_SECRET_ACCESS_KEY=$AWS_SECRET_ACCESS_KEY -e AWS_SESSION_TOKEN=$AWS_SESSION_TOKEN -e AWS_ACCESS_KEY_ID=$AWS_ACCESS_KEY_ID  \
   -e OrleansSettings__MembershipTable=$OrleansSettings__MembershipTable -e OrleansSettings__AwsRegion=$OrleansSettings__AwsRegion --rm client-cluster 