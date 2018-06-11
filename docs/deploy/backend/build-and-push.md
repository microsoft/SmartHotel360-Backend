# Deploy services on AKS: Build and push images

## Pre-requisites

1. AKS created, up & running & kubectl configured to use it.
2. Helm installed locally, Tiller installed on cluster
3. If using custom images: [images built and pushed in a ACR](./build-and-push.md)ç

Next steps:

4. If using custom images: [docker login secret loaded into the cluster](./deploy-secret.md)
5. [Create resources in cluster](./create-resources.md)¡

**All tasks must be performed from `/src/SmartHotel360-Azure-backend/deploy/k8s` folder**

## Build and push the images on ACR

The `build-push.sh` file builds all Docker images (using docker multi-stage build, so you don't need to have anything installed but Docker) and then pushes in the specified ACR registry. **This script assumes you are logged using `az` against the correct Azure subscription**.

This script has the following parameters:

* `-a <acr-name>`: Name (not FQDN) of the ACR to use. Defaults to the `ACR_NAME` env value.
* `-r <fqdn>`: FQDN of the Docker registry to use. If this value is set, it overrides the `ACR_NAME` and `-a` parameter and allows deploys to non ACR registries.
* `-t`: Docker tag image to push and deploy in the cluster. Defaults to current Git branch
* `-o`: Docker organization of the images. Defaults to `smarthotels`.
* `--user <login>`: Needed if pushing to a non ACR registry (if `-r` used). Is the login of the Docker Registry
* `--password <login>`: Needed if pushing to a non ACR registry (if `-r` used). Is the password of the Docker Registry
* `--no-build`: If passed images are not built locally
* `--no-push`: If passed images are not pushed to the Docker registry

So, to build and push the images against an ACR called `my-acr` using the tag `dev` you should type:

```
./build-push.sh -a my-acr -t dev
```

Go to [Deploy Backend services on AKS main topic](../02-depkoy-apis.md)
