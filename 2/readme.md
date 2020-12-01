# readme

In this basic setup we still only have one silo and localhost clustering, but now we have one grain and one client.

The single grain we have is responsible for greeting its clients when called:

`Hello Piotr!`

We have several projects in this solution:

* `SiloHost` - responsible for hosting grains.
* `Interfaces` - this is where grain interfaces are defined. To be used by grain classes and also by the clients.
* `Grains` - this is where grain interface implementations live.
* `Client` - this project demonstrates how to connect to the silo and its grains. Two important things to be pointed out here:
    * `ClusterClientHostedService` - this is a reusable client class common for most orleans clients.
    * `HelloWorldClientHostedService` - to avoid basic, yet annoying problems with e.g. keeping the console alive while waiting for the cluster (of one silo) to become operational, instantiating the `ClusterClientHostedService` at the right time, etc. this illustration is made a hosted service where the orleans client is provided through DI. Simple and elegant.
    
## todo

At the moment the code only works locally (not in docker). Docker support should be added.

* Localhost clustering does not work with docker. Use more configurable types of clustering to allow for inter-container communication.
* Containerize the client.
* Make sure that clients and clusters can communicate in all 4 combinations:
    * local client - local cluster
    * dockerized client - local cluster
    * dockerized client - dockerized cluster
    * local client - dockerized cluster