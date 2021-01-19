#!/bin/bash
export ISLOCAL=true
export MEMBERSHIPTABLE="test-orleans-table"
export AWSREGION="us-west-2"
export CLUSTER_ID="cluster-of-silos"

dotnet run --project ./Api/Api.csproj