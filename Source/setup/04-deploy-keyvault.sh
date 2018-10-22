#!/bin/bash

# Creates an Azure KeyVault service
# Requirement:
#   00-set-vars.sh must be executed Or Set Env. Var. described below
# Environment Variables:
#	SH360_APPNAME: Name of the KeyVault service
#   AKS_RG: Name of the resource group
#   AKS_REGION: location
# Usage:
#	./deploy-keyvault.sh

KeyVaultName="${AKS_NAME}"
aksRg="${AKS_RG}"
aksRegion="${AKS_REGION}"

echo "------------------------------------------------------------"
echo "Creating the KeyVault $KeyVaultName" in rg $aksRg
echo "------------------------------------------------------------"
az provider register -n Microsoft.KeyVault
az keyvault create --name $KeyVaultName --resource-group $aksRg --location $aksRegion
