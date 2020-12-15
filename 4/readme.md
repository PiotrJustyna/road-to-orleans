# readme
# Demo startup
* `./run-docker-silo.sh` and * `./run-local-silo.sh` to create silos, make sure to increment ports.
* `./run-demo.sh` will start both a local and docker client which demonstrates how a single gateway can be used to attach to a cluster of silos.
* To kill ctrl+z and `kill -term [pid]`, containers will have to be manually stopped.
# Individual Startup steps
* `./run-docker-silo.sh`
* Increment the port numbers with the exception of PRIMARYSILOPORT and rerun scripts, any number of silos can be introduced
* `./run-docker-client.sh` or `./run-local-client.sh` to run against one of the silo gateway. 
* ./run-silo-docker.sh to introduce local non docker silos, choose unused ports as we do with the docker example.
* To monitor the containers, you can check the ui view.

## Architecture
* Each client points at a gateway which in turn redistributes the requests to individual silos if needed.
![Cluster of silos](imgs/cluster.png)