# Deploy Backend services on AKS

**Note**: All commands from this doc must be run from `/setup` folder

## Deploy the services in the cluster

Run the `02-deploy-apis.sh` to build backend Docker images and deploy them on cluster. Script accept following parameters:

* `-a <name>`: ACR name to use. Defaults to the value of `ACR_NAME` environment variable.
* `-r <fqdn>`: FQDN of the Docker registry to use. If this value is set, it overrides the `ACR_NAME` and `-a` parameter and allows deploys to non ACR registries.
* `--aks-rg <name>`: Name of the resource group to use. Defaults to `AKS_RG` env variable.
* `--aks-name <name>`: Name of the AKS to use. Defaults to `AKS_NAME` env variable **and it is only used to show the dashboard**.
* `--acr-no-create`: If passed do not create the ACR
* `-t`: Docker tag image to push and deploy in the cluster. Defaults to current Git branch
* `-o`: Docker organization of the images. Defaults to `smarthotels`.
* `-n`: Name of the SmartHotel360 application to be deployed. Defaults to the `SH360_APPNAME` env value.
* `--user <login>`: Needed if pushing to a non ACR registry (if `-r` used). Is the login of the Docker Registry
* `--password <password>`: Needed if pushing to a non ACR registry (if `-r` used). Is the password of the Docker Registry
* `--no-clean`: By default script cleans all Helm releases of the cluster. Passing this parameter avoids this cleanup.
* `--release`: Base name to be used for the Helm releases. If not passed every Helm release has a random name.

This scripts does the following:

1. Creates the ACR (unless `-r` or `--acr-no-create` used)
2. Runs the `/src/SmartHotel360-Azure-backend/deploy/k8s/build-push.sh` script to [build and push the backend services](../../src/SmartHotel360-Azure-backend/docs/build-and-push.md).
3. Runs the `/src/SmartHotel360-Azure-backend/deploy/k8s/deploy-secret.sh` script to [Deploy login information in the cluster](../../src/SmartHotel360-Azure-backend/docs/deploy-secret.md)
4. Runs the `/src/SmartHotel360-Azure-backend/deploy/k8s/clean.sh` script to [clean the Kubernetes cluster](../../src/SmartHotel360-Azure-backend/docs/clean-cluster.md), unless the `--no-clean` is used.
5. Runs the  `/src/SmartHotel360-Azure-backend/deploy/k8s/deploy.sh` to [deploy the Backend services using Helm](.backend/deploy.md).

