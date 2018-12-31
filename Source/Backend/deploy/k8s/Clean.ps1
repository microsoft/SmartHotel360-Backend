Write-Host "cleaning all helm releases from cluster" -ForegroundColor Yellow
# Equivalent in bash of: helm ls --short | xargs -L1 helm delete --purge

helm del $(helm ls --all --short) --purge