# SmartHotel360 Backend Architecture

The following picture describes the backend architecture of the SmartHotel360 services:

![Architecture diagram](./architecture.png)

All backend services are docker images that run on a Kubernetes cluster deployed in Azure using "[Azure Container Service](https://azure.microsoft.com/en-us/services/container-service/)". There are services on various technologies (nodejs, java & netcore).

## Azure resources used

Althought most of the backend services can run locally (using directly Docker or [MiniKube](https://github.com/kubernetes/minikube)) there are some dependencies on Azure resources for some specific scenarios:

* CosmosDB & Azure Storage are used in the "bring your pet" scenario. There are **no locally equivalents** of these resources.
* Azure function is used in the "bring your pet" scenario. In these case it is possible to run & debug the Azure locally, but the CosmosDB & Azure Storage needed must be created.

## For more info...

* [How to run services locally](./run-locally.md)
* [How to run deploy services on azure](./run-azure.md)

[Return to readme](../README.md)
