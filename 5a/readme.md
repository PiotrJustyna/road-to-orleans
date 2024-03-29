# readme

This solution builds on top of solution 5 and is written exclusively in F#.

## running the code

### silos

As in example 4, following scripts can be used to run silos:

- `./run-silo-docker.sh`
- `./run-silo-local.sh`
  An important thing to note is that if one wants to run multiple silos which form a cluster, the following variables need to be made unique for every silo:
- `GATEWAYPORT` (e.g. `3001`, `3002`, etc.)
- `SILOPORT` (e.g. `2001`, `2002`, etc.)
- `DASHBOARDPORT` (e.g. `8081`, `8082`, etc.)

### clients

As in example 4, following scripts can be used to run clients:

- `./run-client-docker.sh`
- `./run-client-local.sh`

### demo

Alternatively, the following scripts can be run to illustrate the rich array of various local/docker silo/client scenarios:

- run the cluster: `./run-demo-cluster.sh` (3 silos: 2 hosted in docker, 1 hosted locally)
- run the clients: `./run-demo-clients.sh` (6 clients: 5 hosted in docker, 1 hosted locally)
- stop the demo: `./stop-demo.sh` (stops all docker containers created during the demo)
