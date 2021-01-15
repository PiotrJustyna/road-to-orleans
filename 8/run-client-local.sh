#!/bin/bash
export ISLOCAL=true
export MEMBERSHIPTABLE="test-orleans-table"
export AWSREGION="us-west-2"

dotnet run --project ./Api/Api.csproj