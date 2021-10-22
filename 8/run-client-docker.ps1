$env:OrleansSettings__AdvertisedIp=( Get-NetIPConfiguration | Where-Object { $_.IPv4DefaultGateway -ne $null -and $_.NetAdapter.Status -ne "Disconnected" } ).IPv4Address.IPAddress
$env:OrleansSettings__GatewayPort=3001
$env:OrleansSettings__MembershipTable="test-orleans-table"
$env:OrleansSettings__AwsRegion="us-west-2"
$env:AWS_REGION=
( get-content ~\.aws\credentials |  foreach-object{ if( $_ -match "region") { $_.substring($_.IndexOf('=') + 1); } } )
$env:AWS_CREDENTIALS=
( get-content ~\.aws\credentials |  foreach-object{ if ($_.Split('_')[0] -match "aws") { $_.substring($_.IndexOf('=') + 1); } } )

$env:AWS_ACCESS_KEY_ID=($env:AWS_CREDENTIALS.Split(''))[0]
$env:AWS_SECRET_ACCESS_KEY=($env:AWS_CREDENTIALS.Split(''))[1]
$env:AWS_SESSION_TOKEN=($env:AWS_CREDENTIALS.Split(''))[2]

"OrleansSettings__MembershipTable" + "=" + $env:OrleansSettings__MembershipTable | Out-File -encoding ascii ./.env
"OrleansSettings__AwsRegion" + "=" + $env:OrleansSettings__AwsRegion | Out-File -Append -encoding ascii ./.env
"AWS_REGION" + "=" + $env:AWS_REGION | Out-File -Append -encoding ascii ./.env
"AWS_ACCESS_KEY_ID"  + "=" + $env:AWS_ACCESS_KEY_ID | Out-File -Append -encoding ascii ./.env
"AWS_SECRET_ACCESS_KEY"  + "=" + $env:AWS_SECRET_ACCESS_KEY | Out-File -Append -encoding ascii ./.env
"AWS_SESSION_TOKEN" + "=" + $env:AWS_SESSION_TOKEN | Out-File -Append -encoding ascii ./.env



docker build -t client-cluster -f ./ops/Api/Dockerfile ./ ; # <-- the ; chains the commands together, like & in bash
docker run -it --env-file ./.env  -p 5432:80 --rm client-cluster
