#!/bin/bash
RetrieveIp(){
  ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'
}

OrleansHostSettings__AdvertisedIp=`RetrieveIp`
OrleansHostSettings__GatewayPort=3001
OrleansHostSettings__DashboardPort=8081
OrleansHostSettings__SiloPort=2001
OrleansHostSettings__MembershipTableName="some-table-name"
#Can also optionally setup a local dynamo db and point at it like so http://$ADVERTISEDIP:8042
OrleansHostSettings__AwsRegion="us-west-2"
OrleansHostSettings__IsLocal=true

AWS_REGION=$(awk -F '=' '/^region/ { print $2 }' ~/.aws/credentials)
AWS_SECRET_ACCESS_KEY=$(awk -F '=' '/^aws_secret_access_key/ { print $2 }' ~/.aws/credentials)
AWS_SESSION_TOKEN=$(awk -F '=' '/^aws_session_token/ { print $2 }' ~/.aws/credentials)
AWS_ACCESS_KEY_ID=$(awk -F '=' '/^aws_access_key_id/ { print $2 }' ~/.aws/credentials)



docker build -t silo-host-cluster -f ./ops/SiloHost/Dockerfile ./ &&
  docker run -it -e AWS_REGION=$AWS_REGION -e AWS_SECRET_ACCESS_KEY=$AWS_SECRET_ACCESS_KEY -e AWS_SESSION_TOKEN=$AWS_SESSION_TOKEN -e AWS_ACCESS_KEY_ID=$AWS_ACCESS_KEY_ID\
  -e OrleansHostSettings__AdvertisedIp=$OrleansHostSettings__AdvertisedIp  -e OrleansHostSettings__GatewayPort=$OrleansHostSettings__GatewayPort -e OrleansHostSettings__SiloPort=$OrleansHostSettings__SiloPort -e OrleansHostSettings__DashboardPort=$OrleansHostSettings__DashboardPort\
   -e OrleansHostSettings__IsLocal=$OrleansHostSettings__IsLocal -p $OrleansHostSettings__GatewayPort:$OrleansHostSettings__GatewayPort -p $OrleansHostSettings__SiloPort:$OrleansHostSettings__SiloPort -p $OrleansHostSettings__DashboardPort:$OrleansHostSettings__DashboardPort  -e OrleansHostSettings__MembershipTableName=$OrleansHostSettings__MembershipTableName -e OrleansHostSettings__AwsRegion=$OrleansHostSettings__AwsRegion --rm silo-host-cluster