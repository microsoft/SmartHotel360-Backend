# Creates an Azure KeyVault service
# Requirement:
#   00-set-vars.sh must be executed Or Set Env. Var. described below
# Environment Variables:
#	SH360_APPNAME: Name of the KeyVault service
#   AKS_RG: Name of the resource group
#   AKS_REGION: location
# Usage:
#	./deploy-keyvault.sh

$KeyVaultName="$env:AKS_NAME"
$aksRg="$env:AKS_RG"
$aksRegion="$env:AKS_REGION"

Write-Host "------------------------------------------------------------"
Write-Host "Creating the KeyVault $KeyVaultName" in rg $aksRg
Write-Host "------------------------------------------------------------"
az provider register -n Microsoft.KeyVault
az keyvault create --name $KeyVaultName --resource-group $aksRg --location $aksRegion