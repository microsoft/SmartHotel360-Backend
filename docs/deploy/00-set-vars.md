# Setup environment

**Note**: All commands from this doc must be run from `/setup` folder

Before begining you need to setup your environmment. Go to `/setup` folder and run the `00-set-vars.sh`. This script sets some environment variables that are used in following steps.

**Note**: You have to use `source` to run the script to ensure that exported variables are available in your terminal session.

The script expects following parameters:

* `-g <resource-group>`: Resource group to use
* `-s <subscription>`: Azure Subscription to use
* `-n <name>`: AKS cluster name to be used
* `-r <name>`: ACR name to be used (just name, not FQDN)
* `-l <location>`: Location to be used. Defaults to `eastus`
* `-c <spn-client>`: Service principal app id
* `-p <spn-pwd>`: Service principal password
* `-a <name>`: Name of the Sh360 app to be installed in the cluster. Defaults to  `myapp`

This scripts set the following environment variables:

```
AKS_SUB
AKS_RG
AKS_NAME
ACR_NAME
AKS_REGION
SPN_CLIENT_ID
SPN_PW
SH360_APPNAME
```

Those variables are used to default parameters in following scripts. In some scripts you can override them

Next step: [Create AKS](./01-aks-create-md)