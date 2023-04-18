# readme

This is the most basic setup: only one silo, localhost clustering, no clients, one reminder grain. For the purpose of the exercise, it will be acting on its own and not part of a distributed cluster

## run

To run, use any of the methods listed below:

* `./run-silo-local.sh`
* `./run-silo-docker.sh`
* run it from your IDE

To verify everything is working correctly:

* Dashboard: http://localhost:8081
* Dashboard if running with Docker http://localhost:3000 

Provided the ports are not changed.

This project also supports dev containers and has the required files to run within dev containers. See  [here](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers) for explanation of dev containers