$env:OrleansHostSettings__AdvertisedIp=
( Get-NetIPConfiguration |
        Where-Object {
            $_.IPv4DefaultGateway -ne $null -and
                    $_.NetAdapter.Status -ne "Disconnected"
        }
).IPv4Address.IPAddress

$env:OrleansHostSettings__GatewayPort="3002"
$env:OrleansHostSettings__DashboardPort="8082"
$env:OrleansHostSettings__SiloPort="2002"
$env:OrleansHostSettings__MembershipTableName="test-orleans-table"
$env:OrleansHostSettings__AwsRegion="us-west-2"
$env:OrleansHostSettings__IsLocal="true"


$env:AWS_REGION=( get-content ~\.aws\credentials |  foreach-object{ if( $_ -match "region") { $_.substring($_.IndexOf('=') + 1); } } )
$env:AWS_CREDENTIALS=( get-content ~\.aws\credentials |  foreach-object{ if ($_.Split('_')[0] -match "aws") { $_.substring($_.IndexOf('=') + 1); } } )
$env:AWS_ACCESS_KEY_ID=($env:AWS_CREDENTIALS.Split(''))[0]
$env:AWS_SECRET_ACCESS_KEY=($env:AWS_CREDENTIALS.Split(''))[1]
$env:AWS_SESSION_TOKEN=($env:AWS_CREDENTIALS.Split(''))[2]
 
# --no-launch-profile ignores launchSettings.json 
dotnet run --no-launch-profile --project ./SiloHost/SiloHost.csproj
