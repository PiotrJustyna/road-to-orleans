#!/bin/bash
export OrleansSettings__IsLocal=true
export OrleansSettings__MembershipTable="some-table-name"
export OrleansSettings__AwsRegion="us-west-2"

AWS_REGION=$(awk -F '=' '/^region/ { print $2 }' ~/.aws/credentials)
AWS_SECRET_ACCESS_KEY=$(awk -F '=' '/^aws_secret_access_key/ { print $2 }' ~/.aws/credentials)
AWS_SESSION_TOKEN=$(awk -F '=' '/^aws_session_token/ { print $2 }' ~/.aws/credentials)
AWS_ACCESS_KEY_ID=$(awk -F '=' '/^aws_access_key_id/ { print $2 }' ~/.aws/credentials)

dotnet run --no-launch-profile --project ./Api/Api.csproj