#!/bin/bash
RetrieveIp(){
  ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v -m1 '127.0.0.1'
}

export OrleansHostSettings__AdvertisedIp=`RetrieveIp`
export OrleansHostSettings__GatewayPort=3001
export OrleansHostSettings__DashboardPort=8081
export OrleansHostSettings__SiloPort=2001
export OrleansHostSettings__MembershipTableName="some-table-name"
export OrleansHostSettings__AwsRegion="us-west-2"
export OrleansHostSettings__IsLocal=true

export AWS_REGION=$(awk -F '=' '/^region/ { print $2 }' ~/.aws/credentials)
export AWS_SECRET_ACCESS_KEY=$(awk -F '=' '/^aws_secret_access_key/ { print $2 }' ~/.aws/credentials)
export AWS_SESSION_TOKEN=$(awk -F '=' '/^aws_session_token/ { print $2 }' ~/.aws/credentials)
export AWS_ACCESS_KEY_ID=$(awk -F '=' '/^aws_access_key_id/ { print $2 }' ~/.aws/credentials)

dotnet run --no-launch-profile --project ./SiloHost/SiloHost.csproj