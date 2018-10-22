# Deploy public web on AKS

Run the `03-deploy-web.sh` script to build and deploy the public web in the cluster. This script accept following parameters:

* `-a <name>`: ACR name to use. Defaults to the value of `ACR_NAME` environment variable.
* `-r <fqdn>`: FQDN of the Docker registry to use. If this value is set, it overrides the `ACR_NAME` and `-a` parameter and allows deploys to non ACR registries.
* `-t`: Docker tag image to push and deploy in the cluster. Defaults to current Git branch
* `-o`: Docker organization of the images. Defaults to `smarthotels`.
* `-n`: Name of the SmartHotel360 application to be deployed. Defaults to the `SH360_APPNAME` env value.
* `--user <login>`: Needed if pushing to a non ACR registry (if `-r` used). Is the login of the Docker Registry
* `--password <password>`: Needed if pushing to a non ACR registry (if `-r` used). Is the password of the Docker Registry
* `--release`: Base name to be used for the Helm releases. If not passed every Helm release has a random name.
* `--clean`: If passed all Helm releases will be removed from the cluster
* `--no-build`: If passed web image is not built locally
* `--no-push`: If passed web image is not pushed to the Docker registry

