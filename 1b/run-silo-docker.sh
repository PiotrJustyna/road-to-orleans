#!/bin/bash

docker build -t silo-host-cluster -f ./ops/Dockerfile ./ &&
  docker run -it -p 3000:8080 --rm silo-host-cluster