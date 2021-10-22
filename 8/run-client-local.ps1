$env:OrleansSettings__MembershipTable="test-orleans-table"
$env:OrleansSettings__AwsRegion="us-west-2"
$env:OrleansSettings__IsLocal="true"

dotnet run --no-launch-profile --project ./Api/Api.csproj
