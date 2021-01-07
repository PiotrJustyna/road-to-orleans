#!/bin/bash
docker build -t vegeta-loadtest -f ./ops/Vegeta/Dockerfile ./ &&
  docker run -it --rm vegeta-loadtest