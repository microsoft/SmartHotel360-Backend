Create AKS with the following features:

* Log analytics workspace
* Container health
* HTTP application routing
* AKS Kuberentes v1.9.6


```
export SPN_PW=
export SPN_CLIENT_ID=4f8dc533-caad-41c1-9ac2-00dcadd39c7c

export AKS_SUB=dd323474-a5cb-40c9-9360-3dbc04a7cf90
export AKS_RG=build-demo
export AKS_NAME=build-demo-2
```

## Create 1.9.6 AKS Cluster

```
az group deployment create -n 01-aks-create -g ${AKS_RG} --template-file 01-aks-create.json \
        --parameters \
        resourceName=${AKS_NAME} \
        dnsPrefix=${AKS_NAME} \
        servicePrincipalClientId=${SPN_CLIENT_ID} \
        servicePrincipalClientSecret=${SPN_PW} \
        workspaceRegion="Canada Central" \
        enableHttpApplicationRouting=true \
        enableOmsAgent=true
```

## GET AKS Cluster

```
az resource show --api-version 2018-03-31 \
        --id /subscriptions/${AKS_SUB}/resourceGroups/${AKS_RG}/providers/Microsoft.ContainerService/managedClusters/${AKS_NAME}
```
