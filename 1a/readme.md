# readme

This is the most basic setup: only one silo, localhost clustering, no clients, one grains. It is ready to participate in a cluster if needed. For the purpose of the exercise, it will be acting on its own.

## run

To run, use any of the methods listed below:

* `./run-silo-local.sh`
* `./run-silo-docker.sh`
* run it from your IDE, but please make sure you have the environment variables set to e.g. `DASHBOARDPORT=8081;GATEWAYPORT=3001;PRIMARYPORT=2001;SILOPORT=2001`

To verify everything is working correctly:

* Dashboard: http://localhost:8081
* API: http://localhost:5000/dummy

Provided the ports are not changed.