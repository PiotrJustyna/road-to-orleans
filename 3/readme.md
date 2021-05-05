# readme

In this basic setup we still only have one silo and localhost clustering with one grain and one client. This time, though, both the client and the silo host are dockerized and multiple modes of communications are supported for developer's convenience.

* local client - local silo host:
    * `./run-silo-local.sh`
    * `./run-client-local.sh`
* local client - dockerized silo host:
    * `./run-silo-docker.sh`
    * `./run-client-local.sh`
* dockerized client - dockerized silo host:
    * `./run-silo-docker.sh`
    * `./run-client-docker.sh`
* dockerized client - local silo host:
    * `./run-silo-local.sh`
    * `./run-client-docker.sh`

Alternatively, one can use the `.vscode/tasks.json` and `.vscode/launch.json` files to build and run local instances of silos and clients.

The way it is all set up is:

* for local runs (local, meaning on the physical machine, non-dockerized), both the client and the silo host get the local ip address of the machine they are running on. They use that IP to communicate with each other.
* for dockerized runs, both the client and the silo host get the local ip address from environment variable `ADVERTISEDIP` which is in turn provided by the convenience run scripts:
    * `./run-client-docker.sh`
    * `./run-silo-docker.sh`

The single grain we have is responsible for greeting its clients when called:

`Hello Piotr!`

We have several projects in this solution:

* `SiloHost` - responsible for hosting grains.
* `Interfaces` - this is where grain interfaces are defined. To be used by grain classes and also by the clients.
* `Grains` - this is where grain interface implementations live.
* `Client` - this project demonstrates how to connect to the silo and its grains. Two important things to be pointed out here:
    * `ClusterClientHostedService` - this is a reusable client class common for most orleans clients.
    * `HelloWorldClientHostedService` - to avoid basic, yet annoying problems with e.g. keeping the console alive while waiting for the cluster (of one silo) to become operational, instantiating the `ClusterClientHostedService` at the right time, etc. this illustration is made a hosted service where the orleans client is provided through DI. Simple and elegant.
