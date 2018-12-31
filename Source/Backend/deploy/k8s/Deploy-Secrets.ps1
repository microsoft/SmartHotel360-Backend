Param(
    [Parameter(Mandatory=$false)][string] $acrName = $env:ACR_NAME,
    [Parameter(Mandatory=$false)][string] $registry,
    [Parameter(Mandatory=$false)][string] $password = $env:ACR_PASSWORD,
    [Parameter(Mandatory=$false)][string] $user = $env:ACR_LOGIN,
    [Parameter(Mandatory=$false)][string] $secretName = "registry-key"
)

if (-not [string]::IsNullOrEmpty($acrName)) {
    $registry = "$($acrName).azurecr.io"
}

if([string]::IsNullOrEmpty($registry)){
    Write-Host "No docker registry specified. Set ACR_NAME env value to ACR name or use -registry <fqdn-registry>" -ForegroundColor Yellow
  exit 1
}

if([string]::IsNullOrEmpty($password)){
    Write-Host "No password specified. Use -pasword <password> or set ACR_PASSWORD env value" -ForegroundColor Yellow
  exit 1
}

if([string]::IsNullOrEmpty($user)){
    Write-Host "No docker user specified. Use -user <docker-user> or set ACR_LOGIN env value" -ForegroundColor Yellow
  exit 1
}

Write-Host "Creating secret named $secretName for storing docker user credentials for registry $registry" -ForegroundColor Yellow

kubectl delete secret $secretName
kubectl create secret docker-registry $secretName --docker-server $registry --docker-username $user --docker-password $password --docker-email "not@used.com"