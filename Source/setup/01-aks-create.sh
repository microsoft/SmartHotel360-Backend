#!/bin/bash

. show-env.sh

echo "------------------------------------------------------------"
echo "Creating Resource Group ${AKS_RG}"
echo "------------------------------------------------------------"
az group create -n ${AKS_RG} -l ${AKS_REGION}

echo "------------------------------------------------------------"
echo "Creating KeyVault ${AKS_NAME} in Resource Group ${AKS_RG}"
echo "------------------------------------------------------------"
. 04-deploy-keyvault.sh

cd ../provision

echo "------------------------------------------------------------"
echo "Creating Cluster ${AKS_NAME} in Resource Group ${AKS_RG}"
echo "------------------------------------------------------------"
az group deployment create -n ${AKS_NAME}create -g ${AKS_RG} --template-file 01-aks-create.json --parameters resourceName=${AKS_NAME} dnsPrefix=${AKS_NAME} servicePrincipalClientId=${SPN_CLIENT_ID} servicePrincipalClientSecret=${SPN_PW} workspaceRegion="Canada Central" enableHttpApplicationRouting=true enableOmsAgent=true

echo "------------------------------------------------------------"
echo "Getting credentials for cluster ${AKS_NAME}"
echo "------------------------------------------------------------"
az aks get-credentials -n ${AKS_NAME} -g ${AKS_RG}

echo "------------------------------------------------------------"
echo "Initializing Helm"
echo "------------------------------------------------------------"
helm init

cd ../setup
