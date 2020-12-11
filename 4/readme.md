# readme
# Startup steps
* `./run-docker-silo.sh`
* Increment the port numbers with the exception of PRIMARYSILOPORT and rerun scripts, any number of silos can be introduced
* `./run-docker-client.sh` or `./run-local-client.sh` to run against one of the silo gateway. 
* ./run-silo-docker.sh to introduce local non docker silos, choose unused ports as we do with the docker example.
* To monitor the containers, you can check the ui view.

## Architecture
* Each client points at a gateway and redistributes the requests to individual silos. 
![Cluster of silos](imgs/cluster.png)