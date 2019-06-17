# Deploy Backend services on AKS

**Note**: All commands from this doc must be run from `/setup` folder

## Deploy the services in the cluster

From Bash terminal: Run the `02-deploy-apis.sh` to build backend Docker images and deploy them on cluster. Script accept following parameters:

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
* `--httpRouting`: pass it if the cluster is an AKS with HTTP Routing Enabled. In this case the script auto-detects the dns.
* `--dns`: Name of the dns to use in case we do not use `--httpRouting`

From Powershell terminal: Run the `02-deploy-apis.ps1` to build backend Docker images and deploy them on cluster. Script accept following parameters:

* `-acrName <name>`: ACR name to use. Defaults to the value of `ACR_NAME` environment variable.
* `-registry <fqdn>`: FQDN of the Docker registry to use. If this value is set, it overrides the `ACR_NAME` and `-acrName` parameter and allows deploys to non ACR registries.
* `-aksRg <name>`: Name of the resource group to use. Defaults to `AKS_RG` env variable.
* `-aksName <name>`: Name of the AKS to use. Defaults to `AKS_NAME` env variable **and it is only used to show the dashboard**.
* `-createAcr` <$true OR $false>: when false, does not create a new ACR. Defaults to true (create an ACR). 
* `-imageTag`: Docker tag image to push and deploy in the cluster. Defaults to current Git branch
* `-dockerOrg`: Docker organization of the images. Defaults to `smarthotels`.
* `-appName`: Name of the SmartHotel360 application to be deployed. Defaults to the `SH360_APPNAME` env value.
* `-customLogin <login>`: Needed if pushing to a non ACR registry (if `-registry` used). Is the login of the Docker Registry
* `-customPassword <password>`: Needed if pushing to a non ACR registry (if `-registry` used). Is the password of the Docker Registry
* `-clean` <$true OR $false>: By default script cleans all Helm releases of the cluster. Passing this parameter with false avoids this cleanup.
* `-httpRouting` <$true OR $false>: pass it with true if the cluster is an AKS with HTTP Routing Enabled. In this case the script auto-detects the dns.
* `-dns`: Name of the dns to use in case we do not use `--httpRouting`

This scripts does the following:

1. Creates the ACR (unless `-r` or `--acr-no-create` used)
2. Runs the `/Source/Backend/deploy/k8s/build-push.sh` script to [build and push the backend services](./backend/build-and-push.md).
3. Runs the `/Source/Backend/deploy/k8s/deploy-secret.sh` script to [deploy login information in the cluster](./backend/deploy-secret.md)
4. Runs the `/Source/Backend/deploy/k8s/clean.sh` script to [clean the Kubernetes cluster](./backend/clean-cluster.md), unless the `--no-clean` is used.
5. Runs the  `/Source/Backend/deploy/k8s/deploy.sh` to [deploy the Backend services using Helm](./backend/deploy.md).

