# readme

In this clustered setup, we have moved it to an entirely dockerised setup to allow for easy scaling.

* 2 clients - 2 silos:
    * `./run-docker-silo-cluster.sh`

The way it is all set up is:

# Fully Dockerised 
* Two silos are created:
    * Each basically act as a node of the cluster
    * To simulate the cluster, the second silo attaches itself to the first silo.
* Two clients are created
    * Both gateways are visible to the clients, an intermittent health check is run against to confirm they are alive.

# Dockerised Silos with local connection
* Two silos are created in docker
    * They're fully visible to non docker silos however local clients can only utilise the local silos.


