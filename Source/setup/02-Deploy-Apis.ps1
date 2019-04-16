Param(
    [Parameter(Mandatory=$false)][string] $registry,
    [Parameter(Mandatory=$false)][string] $acrName = $env:ACR_NAME,
    [Parameter(Mandatory=$false)][string] $imageTag = (git rev-parse --abbrev-ref HEAD),
    [Parameter(Mandatory=$false)][string] $dockerOrg = "smartHotels",
    [Parameter(Mandatory=$false)][string] $appName = $env:SH360_APPNAME,
    [Parameter(Mandatory=$false)][string] $customLogin = "",
    [Parameter(Mandatory=$false)][string] $customPassword = "",
    [Parameter(Mandatory=$false)][string] $aksName = $env:AKS_NAME,
    [Parameter(Mandatory=$false)][string] $aksRg = $env:AKS_RG,
    [Parameter(Mandatory=$false)][boolean] $createAcr = $true,
    [Parameter(Mandatory=$false)][string] $dns = "none",
    [Parameter(Mandatory=$false)][boolean] $httpRouting = $false,
    [parameter(Mandatory=$false)][string][ValidateSet('prod','staging', IgnoreCase=$false)]$tlsEnv = "staging"
)

function validateParams{
    
    if ($httpRouting){
        if([string]::IsNullOrEmpty($aksName)){
            Write-Host "No cluster is specified. Please use -aksName or set the AKS_NAME env value." -ForegroundColor Red
            .\Show-Env.ps1
            exit 1
        }
        if ([string]::IsNullOrEmpty($aksRg)){
            Write-Host "No resource group is specified. Please use -aksRg or set the AKS_RG env value." -ForegroundColor Red
            .\Show-Env.ps1
            exit 1
        }
    }
    
    if($createAcr){
        if ([string]::IsNullOrEmpty($aksRg)){
            Write-Host "No resource group is specified. Please use -aksRg or set the AKS_RG env value." -ForegroundColor Red
            .\Show-Env.ps1
            exit 1
        }
    }
}
    validateParams

if ($httpRouting){
    Write-Host "Use of --httpRouting overrides -d" -ForegroundColor Yellow
    Write-Host "Autodetecting DNS of $aksName in $aksRg" -ForegroundColor Yellow
    $dns = (az resource show --api-version "2018-03-31" --id /subscriptions/$env:AKS_SUB/resourceGroups/$env:AKS_RG/providers/Microsoft.ContainerService/managedClusters/$aksName --query properties.addonProfiles.httpApplicationRouting.config.HTTPApplicationRoutingZoneName)
    $dns = $dns.Replace('"',"")
    Write-Host "DNS detected is: $dns" -ForegroundColor Yellow

    if([string]::IsNullOrEmpty($dns)){
        Write-Host "No DNS could be auto-detected. Ensure cluster is AKS with HTTP Routing Enabled & AZ CLI is properly configured" -ForegroundColor Yellow
        exit 1
    }

    $dns="$appName.$dns"
}

if((-not [string]::IsNullOrEmpty($acrName)) -AND [string]::IsNullOrEmpty($registry)){
    $registry = ("$($acrName).azurecr.io")
}

Push-Location ../Backend/deploy/k8s

if(-not [string]::IsNullOrEmpty($acrName)){
    if($createAcr){
        Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
        Write-Host "Creating the registry $acrName in rg $aksRg" -ForegroundColor Yellow
        Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
        az acr create -n $acrName -g $aksRg --admin-enabled true --sku Basic
    }
    else {
        Write-Host "Skipping ACR creation for $acrName (--createAcr equals to true)" -ForegroundColor Yellow
    }

    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    Write-Host "Building and pushing to ACR $acrName" -ForegroundColor Yellow
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    .\build-push.ps1 -acrName $acrName -imageTag $imageTag

    Write-Host "Creating secret on k8s. Retrieving login & pwd of ACR $acrName" -ForegroundColor Yellow

    $acrPassword=(az acr credential show -n $acrName | ConvertFrom-Json).passwords[0].value 
    $acrLogin=(az acr credential show -n $acrName | ConvertFrom-Json).username

    .\Deploy-Secret.ps1 -registry $registry -password $acrPassword -user $acrLogin
}

if ([string]::IsNullOrEmpty($acrName) -and (-not [string]::IsNullOrEmpty($registry))) {
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    Write-Host "Building and pushing to custom registry $acrName" -ForegroundColor Yellow
    Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
    
    Write-Host "You have specified a custom registry (not ACR). Login and pwd can't be retrieved automatically." -ForegroundColor Yellow
    if([string]::IsNullOrEmpty($customLogin) -and [string]::IsNullOrEmpty($customPassword)){
        Write-Host "Error: Must pass custom registry user (-customLogin) and password (-customPassword)." -ForegroundColor Yellow
        exit 1
    }
    
    .\build-push.ps1 -r $registry -t $imageTag -user $customLogin -password $customPassword
}

Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
Write-Host "Deploying the code from registry '$registry'" -ForegroundColor Yellow
Write-Host "Application Name used: $appName" -ForegroundColor Yellow
Write-Host "Image tag is: $imageTag" -ForegroundColor Yellow
Write-Host "Ingress DNS: $dns" -ForegroundColor Yellow
Write-Host "------------------------------------------------------------" -ForegroundColor Yellow

if (-not [string]::IsNullOrEmpty($registry)) {
    .\deploy.ps1  -registry $registry -release $appName -appName $appName -imageTag $imageTag -dns $dns -tlsEnv $tlsEnv
}
else {
    .\deploy.ps1  -release $appName -appName $appName -imageTag $imageTag -dns $dns -tlsEnv $tlsEnv
}

Pop-Location

Write-Host "------------------------------------------------------------" -ForegroundColor Yellow
Write-Host "Launching Kubernetes dashboard"
Write-Host "------------------------------------------------------------" -ForegroundColor Yellow

az aks browse -n $aksName -g $aksRg