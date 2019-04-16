
.\Show-Env.ps1

Write-Host "------------------------------------------------------------"
Write-Host "Creating Resource Group $env:AKS_RG"
Write-Host "------------------------------------------------------------"
az group create -n $env:AKS_RG -l $env:AKS_REGION

Write-Host "------------------------------------------------------------"
Write-Host "Creating KeyVault $env:AKS_NAME in Resource Group $env:AKS_RG"
Write-Host "------------------------------------------------------------"
.\04-Deploy-Keyvault.ps1

Push-Location ../arm

Write-Host "------------------------------------------------------------"
Write-Host "Creating Cluster in Resource Group $env:AKS_RG"
Write-Host "------------------------------------------------------------"
$outputs=(az group deployment create -n $env:AKS_NAME -g $env:AKS_RG --template-file smarthote360.backend.deployment.json --parameters aksClusterName=$env:AKS_NAME registryName=$env:ACR_NAME servicePrincipalClientId=$env:SPN_CLIENT_ID servicePrincipalClientSecret=$env:SPN_PW)

$aksName=($outputs | ConvertFrom-Json).properties.outputs.aks.value

Write-Host "------------------------------------------------------------"
Write-Host "AKS cluster created with name $aksName"
Write-Host "------------------------------------------------------------"

Write-Host "------------------------------------------------------------"
Write-Host "Getting credentials for cluster $aksName"
Write-Host "------------------------------------------------------------"
az aks get-credentials -n $aksName -g $env:AKS_RG

Write-Host "------------------------------------------------------------"
Write-Host "Initializing Helm"
Write-Host "------------------------------------------------------------"
helm init

Pop-Location
