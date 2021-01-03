# readme

This example builds on top of solution 5.
In this example we substitute the basic console client with a Web Api - the cluster is called from a very simple Web Api containing only one GET action. The example also demonstrates the usage of `CancellationToken`s and the Orleans wrapper `GrainCancellationToken`s.

The demo illustrates the usage of a cluster of silos, one client and a load test running against that client (for the sake of the illustration, we activate a grain for each call the api makes to the silo cluster).

The Web Api's GET action can be accessed by default using the following URL: `http://localhost:5432/helloworld?name=Piotr`.

## running the code

### silos

As in example 5, following scripts can be used to run silos:

* `./run-docker-silo.sh`
* `./run-local-silo.sh`

Important thing to note is that if one wants to run multiple silos which form a cluster, following variables need to be made unique for every silo:

* `GATEWAYPORT` (e.g. `3001`, `3002`, etc.)
* `SILOPORT` (e.g. `2001`, `2002`, etc.)
* `DASHBOARDPORT` (e.g. `8081`, `8082`, etc.)

### clients

As in example 5, following scripts can be used to run clients:

* `./run-client-docker.sh`
* `./run-client-local.sh`

### demo

Now we get to a bit more serious testing as it's easier to laod test the setup. Here is what's happening:

* left: cluster of silos running.
* right: Web Api running.

![1](./imgs/1.png)

Load test results - vegeta:

![2](./imgs/2.png)

Load test results - orleans dashboard:

![3](./imgs/3.png)

Load test results - orleans dashboard:

![4](./imgs/4.png)

Grain distribution across the cluster of silos:

![5](./imgs/5.png)

#### prerequisites

Ideally `vegeta`, but in case it is not installed, `./run-demo-load-test.sh` should be modified to invoke the preferred load tester.

#### running the demo

* run the cluster: `./run-demo-cluster.sh` (3 silos: 2 hosted in docker, 1 hosted locally)
* run the client: `./run-demo-client.sh` (only one client needed in this case, vegeta is going to do the heavy lifting)
* run the load test: `./run-demo-load-test.sh` (vegeta script generating 100 requests per second of constant load)