# readme

In this clustered setup, we have moved it to an entirely dockerised setup to allow for easy scaling.

* 2 clients - 2 silos:
    * `./run-docker-silo-cluster.sh`

The way it is all set up is:

* Two silos are created:
    * The first is a primary silo
    * The second is a secondary one that connects to the primary
* Two clients are created
    * The first connects directly to the primary
    * The second goes to the secondary
    * This simulates a distribution of requests across two silos that are interconnected.