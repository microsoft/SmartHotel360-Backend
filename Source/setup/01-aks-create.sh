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

pushd ../arm

echo "------------------------------------------------------------"
echo "Creating Cluster in Resource Group ${AKS_RG}"
echo "------------------------------------------------------------"
outputs=$(az group deployment create -n ${AKS_NAME} -g ${AKS_RG} --template-file smarthote360.backend.deployment.json --parameters aksClusterName=${AKS_NAME} registryName=${ACR_NAME} servicePrincipalClientId=${SPN_CLIENT_ID} servicePrincipalClientSecret=${SPN_PW})

aksName=$(echo "$outputs" | jq ".properties.outputs.aks.value" | tr -d '""')

echo "------------------------------------------------------------"
echo "Getting credentials for cluster ${aksName}"
echo "------------------------------------------------------------"
az aks get-credentials -n ${aksName} -g ${AKS_RG}

popd

. add-tiller.sh
