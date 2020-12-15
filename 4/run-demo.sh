#!/bin/bash 
# To kill ctrl+z and `kill -term [pid]`, containers will have to be manually stopped
./run-silo-docker.sh 
./run-client-docker.sh 
./run-silo-local.sh &
./run-client-local.sh 