#!/bin/bash 
./ops/Convenience/run-silo-1-docker.sh &
./ops/Convenience/run-silo-3-docker.sh &
./ops/Convenience/run-silo-2-local.sh