# Deploy Backend services on AKS: Deploy docker registry login information

## Pre-requisites

1. AKS created, up & running & kubectl configured to use it.
2. Helm installed locally, Tiller installed on cluster
3. ACR or any other docker registry created

**All tasks must be performed from `/src/SmartHotel360-Azure-backend/deploy/k8s` folder**

## Deploy secret to cluster

In order to use any private Docker registry (ACR or non-ACR) you need to provide the cluster information about the credentials to login the cluster. To do this, run the `deploy-secret.sh` script. This script accepts following parameters:

* `-r <fqdn-registry>`: FQDN of the docker registry (the login server). In ACR it is `<acr-name>.azurecr.io`. Mandatory unless the `ACR_NAME` env value is set.
* `u <login>`: Registry login. Defaults to the `ACR_LOGIN` env value.
* `p <password>`: Registry password. Defaults to `ACR_PASSWORD` env value.
* `--secret-name <name>`: Name of the secret in the k8s cluster. Defaults to `registry-key`

Go to [Deploy Backend services on AKS main topic](../02-depkoy-apis.md)