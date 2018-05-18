Create AKS with the following features:

* Log analytics workspace
* Container health
* HTTP application routing
* AKS Kuberentes v1.9.6


```
export SPN_PW=
export SPN_CLIENT_ID=
export AKS_SUB=
export AKS_RG=
export AKS_NAME=
export ACR_NAME=
```

## Create 1.9.6 AKS Cluster

```
az group deployment create -n ${AKS_NAME}create -g ${AKS_RG} --template-file 01-aks-create.json \
        --parameters \
        resourceName=${AKS_NAME} \
        dnsPrefix=${AKS_NAME} \
        servicePrincipalClientId=${SPN_CLIENT_ID} \
        servicePrincipalClientSecret=${SPN_PW} \
        workspaceRegion="Canada Central" \
        enableHttpApplicationRouting=true \
        enableOmsAgent=true \
        containerRegistryName=${ACR_NAME}
```

## GET AKS Cluster

```
az resource show --api-version 2018-03-31 \
        --id /subscriptions/${AKS_SUB}/resourceGroups/${AKS_RG}/providers/Microsoft.ContainerService/managedClusters/${AKS_NAME}
```
