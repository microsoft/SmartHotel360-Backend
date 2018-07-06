#!/bin/bash

. show-env.sh

cd ../provision
az group create -n ${AKS_RG} -l ${AKS_REGION}
az group deployment create -n ${AKS_NAME}create -g ${AKS_RG} --template-file 01-aks-create.json --parameters resourceName=${AKS_NAME} dnsPrefix=${AKS_NAME} servicePrincipalClientId=${SPN_CLIENT_ID} servicePrincipalClientSecret=${SPN_PW} workspaceRegion="Canada Central" enableHttpApplicationRouting=true enableOmsAgent=true
az aks get-credentials -n ${AKS_NAME} -g ${AKS_RG}
cd ../setup
