# Deploy services on AKS: Build and push images

## Pre-requisites

1. AKS created, up & running & kubectl configured to use it.
2. Helm installed locally, Tiller installed on cluster
3. If using custom images: [images built and pushed in a ACR](./build-and-push.md)ç

Next steps:

4. If using custom images: docker login secret loaded into the cluster
5. [Create resources in cluster](./create-resources.md)

**All tasks must be performed from `/src/SmartHotel360-Azure-backend/deploy/k8s folder`**

## Build and push the images on ACR

The `build-push.sh` file builds all Docker images (using docker multi-stage build, so you don't need to have anything installed but Docker) and then pushes in the specified ACR registry. **This script assumes you are logged using `az` against the correct Azure subscription**.

This script has the following sintax:

```
./build-push.sh [acr-name] [tag] [organization]
```

* `acr-name` is the name of ACR to use. It is a mandatory parameter, however if you don't pass it, the value of `ACR_NAME` environment variable will be used. If the variable is not set, the script will fail.
* `tag` defaults to current git branch 
* `organization` defaults to `smarthotels`.

So, to build and push the images against an ACR called `my-acr` using the tag `dev` you should type:

```
./build-push.sh my-acr dev
```
