# readme

This example builds on top of solution 6 and demonstrates how to:

* work with [Timers](https://dotnet.github.io/orleans/docs/grains/timers_and_reminders.html)

## running the code

### silos

As in example 6, following scripts can be used to run silos:

* `./run-silo-docker.sh`
* `./run-silo-local.sh`

Important thing to note is that if one wants to run multiple silos which form a cluster, following variables need to be made unique for every silo:

* `GATEWAYPORT` (e.g. `3001`, `3002`, etc.)
* `SILOPORT` (e.g. `2001`, `2002`, etc.)
* `DASHBOARDPORT` (e.g. `8081`, `8082`, etc.)

### clients

As in example 6, following scripts can be used to run clients:

* `./run-client-docker.sh`
* `./run-client-local.sh`

### running the demo

* run the cluster: `./run-demo-cluster.sh` (3 silos: 2 hosted in docker, 1 hosted locally)
* run the client: `./run-demo-client.sh` (only one client needed in this case, vegeta is going to do the heavy lifting)