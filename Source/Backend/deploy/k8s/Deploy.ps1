Param(
    [Parameter(Mandatory=$false)][string] $registry,
    [Parameter(Mandatory=$false)][boolean] $clean = $false,
    [Parameter(Mandatory=$false)][string] $imageTag = (git rev-parse --abbrev-ref HEAD),
    [Parameter(Mandatory=$false)][string] $release,
    [Parameter(Mandatory=$false)][string] $dockerOrg = "smarthotels",
    [Parameter(Mandatory=$false)][string] $appName,
    [Parameter(Mandatory=$false)][string] $dns
)

if ($dns -eq "none") {
    $dns = ""
}

if([string]::IsNullOrEmpty($appName)){
    Write-Host "must provide a name using -appName" -ForegroundColor Red
    exit 1
}

Write-Host "Using registry: $registry, tag $imageTag & organization $dockerOrg" -ForegroundColor Yellow
Write-Host "Base name for releases is (empty means random for each release): $release" -ForegroundColor Yellow

Write-Host "Getting the nginx controller ip of the cluster... (this can take a while)" -ForegroundColor Yellow

$nginxip = kubectl get svc -n kube-system | findstr -i "LoadBalancer" | ForEach-Object{ ($_ -replace '\s+',' ' ).split()[3]}

Write-Host "DNS used to ingress resources (if any): '$dns' (Use -dns <dns> to config  DNS for ingress). nginx-ingress IP value detected is: $nginxip" -ForegroundColor Yellow

if($clean){
    Write-Host "cleaning all helm releases from cluster" -ForegroundColor Yellow
    # Equivalent in bash of: helm ls --short | xargs -L1 helm delete --purge
    helm del $(helm ls --all --short) --purge
}

$infra = @("sh360-postgres|postgres", "sh360-sql-data|sql")

foreach ($inf in $infra) {
    $array = $inf.split("|")

    if(-not [string]::IsNullOrEmpty($release)){
        $fullrelease="$release-$($array[1])"
    }

    helm install $($array[0]) --name=$fullrelease --set appName=$appName -f infrastructure_values.yml 
    Write-Host "Installing infrastructure $($array[0]) (helm release name is $fullrelease (blank means random)" -ForegroundColor Yellow
    
}

$arr = @("sh360-hotels|hotels", "sh360-bookings|bookings", "sh360-config|configuration", "sh360-discounts|discounts", "sh360-notifications|notifications", "sh360-profiles|profiles", "sh360-reviews|reviews", "sh360-suggestions|suggestions", "sh360-tasks|tasks")

foreach ($svc in $arr) {
    $array = $svc.split('|');
    $currentImage=$array[1]

    if(-not [string]::IsNullOrEmpty($dockerOrg)){
        $currentImage="$dockerOrg/$currentImage"
    }

    if (-not [string]::IsNullOrEmpty($registry)) {
        $currentImage="$registry/$currentImage"
    }

    if (-not [string]::IsNullOrEmpty($release)) {
        $fullrelease="$release-$($array[1])"
    }

    Write-Host "Installing service $($array[0]) (image $currentImage)" -ForegroundColor Yellow
    Write-Host "helm install $($array[0]) --name=$fullrelease --set image.tag=$imageTag --set image.repository=$currentImage --set appName=$appName --set ingress.enabled=1 --set ingress.hosts={$dns} -f ingress_values.yml -f pull_secrets_conf.yml -f infrastructure_values.yml" 
    cmd /c "helm install $($array[0]) --name=$fullrelease --set image.tag=$imageTag --set image.repository=$currentImage --set appName=$appName --set ingress.enabled=1 --set ingress.hosts={$dns} -f ingress_values.yml -f pull_secrets_conf.yml -f infrastructure_values.yml"
}
