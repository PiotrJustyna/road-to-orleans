#!/bin/bash

docker build -t silo-host -f ./ops/SiloHost/Dockerfile ./ &&
  docker run -it -p 3000:3000 -p 8080:8080 --rm silo-host