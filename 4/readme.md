# readme

In this clustered setup, we have moved it to an entirely dockerised setup to allow for easy scaling.

* 2 clients - 2 silos:
    * `./run-docker-silo-cluster.sh`
    * This instantiates two silos and two clients which communicate. 
    * The dashboard is accessible from localhost:3005
* Local scenario:
    * Configuration of ports for gateways and silos can be managed from shell scripts, it will try the range of ports you offer until it finds an available one.
    * Run the following shell scripts in in
    * `./run-silo-locally.sh`, individual terminal per silo.
    * `./run-client-locally.sh` individual terminal per client
# Fully Dockerised summary
* Two silos are created:
    * Each basically act as a node of the cluster
    * To simulate the cluster, the second silo attaches itself to the first silo.
* Two clients are created
    * Both gateways are visible to the clients, an intermittent health check is run against to confirm they are alive.
## Notes
* Local clients cannot connect to docker silos with internal network configurations as they runs into an ip mismatch.