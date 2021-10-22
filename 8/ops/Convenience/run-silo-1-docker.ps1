$env:ADVERTISEDIP = 
( Get-NetIPConfiguration | 
 Where-Object {
 $_.IPv4DefaultGateway -ne $null -and 
 $_.NetAdapter.Status -ne "Disconnected"
 }
 ).IPv4Address.IPAddress

$env:GATEWAYPORT="3001"
$env:DASHBOARDPORT="8081"
$env:SILOPORT="2001"
$env:MEMBERSHIPTABLE="test-orleans-table"
$env:ISLOCAL="true"
$env:AWS_REGION=
( get-content ~\.aws\credentials |  foreach-object{ if( $_ -match "region") { $_.substring($_.IndexOf('=') + 1); } } )
$env:AWS_CREDENTIALS=
( get-content ~\.aws\credentials |  foreach-object{ if ($_.Split('_')[0] -match "aws") { $_.substring($_.IndexOf('=') + 1); } } )


$env:AWS_ACCESS_KEY_ID=($env:AWS_CREDENTIALS.Split(''))[0]
$env:AWS_SECRET_ACCESS_KEY=($env:AWS_CREDENTIALS.Split(''))[1]
$env:AWS_SESSION_TOKEN=($env:AWS_CREDENTIALS.Split(''))[2]

"OrleansHostSettings__AdvertisedIp" + "=" + $env:ADVERTISEDIP | Out-File -encoding ascii ./.env
"OrleansHostSettings__GatewayPort" + "=" + $env:GATEWAYPORT | Out-File -Append -encoding ascii ./.env
"OrleansHostSettings__SiloPort" + "=" + $env:SILOPORT | Out-File -Append -encoding ascii ./.env
"OrleansHostSettings__DashboardPort" + "=" + $env:DASHBOARDPORT | Out-File -Append -encoding ascii ./.env
"OrleansHostSettings__MembershipTableName" + "=" + $env:MEMBERSHIPTABLE | Out-File -Append -encoding ascii ./.env
"OrleansHostSettings__IsLocal" + "=" + $env:ISLOCAL | Out-File -Append -encoding ascii ./.env
"OrleansHostSettings__AwsRegion" + "=" + $env:AWS_REGION | Out-File -Append -encoding ascii ./.env
"AWS_REGION" + "=" + $env:AWS_REGION | Out-File -Append -encoding ascii ./.env
"AWS_ACCESS_KEY_ID"  + "=" + $env:AWS_ACCESS_KEY_ID | Out-File -Append -encoding ascii ./.env
"AWS_SECRET_ACCESS_KEY"  + "=" + $env:AWS_SECRET_ACCESS_KEY | Out-File -Append -encoding ascii ./.env
"AWS_SESSION_TOKEN" + "=" + $env:AWS_SESSION_TOKEN | Out-File -Append -encoding ascii ./.env


docker build -t silo-host-cluster -f ./ops/SiloHost/Dockerfile ./ ;

docker run -d --env-file ./.env -p 3001:3001 -p 2001:2001 -p 8081:8081 --rm silo-host-cluster

