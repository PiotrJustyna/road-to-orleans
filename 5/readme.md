# readme

This solution builds on top of solution 4 and adds F# as an option for potential library code. We often find ourselves in a situation where we'd like to use F# "business logic" interfaced by C# orleans code. This repository illustrates how to do just that using the `Library` project.

## architecture
Each client (hosted locally or in docker) points at a gateway which in turn redistributes the requests to individual silos (hosted locally or in docker) if needed.
![Cluster of silos](imgs/cluster.png)
## running the code
### silos
As in example 4, following scripts can be used to run silos:
* `./run-docker-silo.sh`
* `./run-local-silo.sh`
  Important thing to note is that if one wants to run multiple silos which form a cluster, following variables need to be made unique for every silo:
* `GATEWAYPORT` (e.g. `3001`, `3002`, etc.)
* `SILOPORT` (e.g. `2001`, `2002`, etc.)
* `DASHBOARDPORT` (e.g. `8081`, `8082`, etc.)
### clients
As in example 4, following scripts can be used to run clients:
* `./run-client-docker.sh`
* `./run-client-local.sh`
### demo
* run the cluster: `./run-demo-cluster.sh` (3 silos: 2 hosted in docker, 1 hosted locally)
* run the clients: `./run-demo-clients.sh (6 clients: 5 hosted in docker, 1 hosted locally)
* stop the demo: `./stop-demo.sh` (stops all docker containers created during the demo)