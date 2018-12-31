Param(
    [Parameter(Mandatory=$false)][string] $registry,
    [Parameter(Mandatory=$false)][string] $acrName = $env:ACR_NAME,
    [Parameter(Mandatory=$false)][string] $aksRg = $env:AKS_RG,
    [Parameter(Mandatory=$false)][string] $dockerOrg = "smarthotels",
    [Parameter(Mandatory=$false)][string] $imageTag = (git rev-parse --abbrev-ref HEAD),
    [Parameter(Mandatory=$false)][string] $customLogin = "",
    [Parameter(Mandatory=$false)][string] $customPassword = "",
    [Parameter(Mandatory=$false)][boolean] $useAcr = $true,
    [Parameter(Mandatory=$false)][boolean] $push = $true,
    [Parameter(Mandatory=$false)][boolean] $build = $true
)

if ((-not [string]::IsNullOrEmpty($acrName)) -and [string]::IsNullOrEmpty($registry)) {
    $registry = "$($acrName).azurecr.io"
}

if([string]::IsNullOrEmpty($registry)){
    Write-Host "Must specify registry using either -acrName <acr-name>, or setting the ACR_NAME env value or using -registry <fqdn-registry> for a non-ACR registry)" -ForegroundColor Yellow
  exit 1
}

if($build){
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    Write-Host "Building Docker images tagged with public tag" -ForegroundColor Yellow
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    $env:TAG = $imageTag
    docker-compose -p .. -f ../../src/docker-compose.yml -f ../../src/docker-compose-tagged.yml build
}
else {
    Write-Host "docker build skipped (--no-build used)" -ForegroundColor Yellow
}

if($useAcr){
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    Write-Host "Logging into ACR $acrName" -ForegroundColor Yellow
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    az acr login -n $acrName -g $aksRg
}
else {
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    Write-Host "Logging into custom registry $registry" -ForegroundColor Yellow
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    if ([string]::IsNullOrEmpty($customLogin) -or [string]::IsNullOrEmpty($customPassword)) {
        Write-Host "Error: Must pass custom registry user (--user) and password (--pwd)." -ForegroundColor Yellow
      exit 1
    }
    docker login -p $customPassword -u $customLogin $registry
}

if($push){
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    Write-Host "Retagging public images to $imageTag and pushing images to $registry/$dockerOrg..." -ForegroundColor Yellow
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    $services= @("bookings", "hotels", "suggestions", "tasks", "configuration", "notifications", "reviews", "discounts", "profiles")
    foreach ($service in $services) {
        $imageFqdn="$registry/$dockerOrg/$service"
        docker tag "smarthotels/$($service):public" "$($imageFqdn):$imageTag"
        docker push "$($imageFqdn):$imageTag"
    }
}
else {
    Write-Host "Push skipped (--no-push used)" -ForegroundColor Yellow
}