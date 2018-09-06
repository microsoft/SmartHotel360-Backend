#!/bin/bash

# Sets secrets to Azure KeyVault service
# Requirement:
#   00-set-vars.sh must be executed
# Usage:
#	./set-secrets-keyvault.sh secret1=value1 secret2=value2
#

KeyVaultName="${SH360_APPNAME}"
aksRg="${AKS_RG}"
aksRegion="${AKS_REGION}"


for ARGUMENT in "$@"
do
    KEY=$(echo $ARGUMENT | cut -f1 -d=)
    VALUE=$(echo $ARGUMENT | cut -f2 -d=)   

    echo "------------------------------------------------------------"
    echo "Setting secret to the KeyVault. Secret: $KEY Value: $VALUE" 
    echo "------------------------------------------------------------"

    az keyvault secret set --vault-name $KeyVaultName --name $KEY --value $VALUE
done

echo "------------------------------------------------------------"
echo "Successfully Completed!"
echo "------------------------------------------------------------"





