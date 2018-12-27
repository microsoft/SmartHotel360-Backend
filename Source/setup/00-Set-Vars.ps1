Param(
    [Parameter(Mandatory=$false)][string] $subscription,
    [Parameter(Mandatory=$false)][string] $resourceGroup,
    [Parameter(Mandatory=$false)][string] $clusterName,
    [Parameter(Mandatory=$false)][string] $location,
    [Parameter(Mandatory=$false)][string] $spnClientId,
    [Parameter(Mandatory=$false)][string] $spnPassword,
    [Parameter(Mandatory=$false)][string] $sh360AppName = "myapp"

)

function validate{
    $valid=$true

    if ([string]::IsNullOrEmpty($subscription)) {
        Write-Host "No subscription Id. Use -subscription to specify subscription." -ForegroundColor Red
        $valid=$false
    }

    if ([string]::IsNullOrEmpty($resourceGroup)) {
        Write-Host "No resource group. Use -resourceGroup to specify resource group." -ForegroundColor Red
        $valid=$false
    }

if ([string]::IsNullOrEmpty($clusterName)) {
        Write-Host "No name for the cluster. Use -clusterName to specify cluster name." -ForegroundColor Red
        $valid=$false
    }

    if ([string]::IsNullOrEmpty($location)) {
        Write-Host "No resources location. Use -location to specify resource location." -ForegroundColor Red
        $valid=$false
    }

    if ([string]::IsNullOrEmpty($spnClientId)) {
        Write-Host "No Service Principal Client Id. Use -spnClientId to specify one." -ForegroundColor Red
        $valid=$false
    }

    if ([string]::IsNullOrEmpty($spnPassword)) {
        Write-Host "No Service Principal Password. Use -spnClientId to specify one." -ForegroundColor Red
        $valid=$false
    }

    if ([string]::IsNullOrEmpty($sh360AppName)) {
        Write-Host "No Smart Hotel 360 application name. Use -sh360AppName to specify one." -ForegroundColor Red
        $valid=$false
    }

    if ($valid -eq $false) {
        exit 1
    }
}

if($PSBoundParameters.Count -eq 0){

    Write-Host "Usage: 00-Set-Vars.ps1  -parameter value. Parameters are:" -ForegroundColor Yellow
    Write-Host "   -resourceGroup: Your resource group name" -ForegroundColor Yellow
    Write-Host "   -subscription: Subscription id" -ForegroundColor Yellow
    Write-Host "   -clusterName: Cluster name" -ForegroundColor Yellow
    Write-Host "   -location: Location (defaults to eastus)" -ForegroundColor Yellow
    Write-Host "   -spnclientId: Service principal app id" -ForegroundColor Yellow
    Write-Host "   -spnPassword: Service principal pwd" -ForegroundColor Yellow
    Write-Host "   -sh360AppName: Name of the sh360 application to install (defaults to myapp)" -ForegroundColor Yellow

    exit 1
}

validate

$env:AKS_SUB = $subscription
$env:AKS_RG = $resourceGroup
$env:AKS_NAME = $clusterName
$env:AKS_REGION = $location
$env:SPN_CLIENT_ID = $spnClientId
$env:SPN_PW = $spnPassword
$env:SH360_APPNAME = $sh360AppName

.\Show-Env.ps1