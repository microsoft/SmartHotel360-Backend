export SPN_PW=<service principal password> \ 
export SPN_CLIENT_ID=<service principal app id> \ 
export AKS_SUB=<azure subscription id> \ 
export AKS_RG=<resource group name> \ 
export AKS_NAME=<AKS cluster name> \ 
export ACR_NAME=<Azure Container Registry name> \ 
export AKS_REGION=eastus

az group create -n ${AKS_NAME} -l ${AKS_REGION}

az group deployment create -n ${AKS_NAME}create -g ${AKS_RG} --template-file 01-aks-create.json \
        --parameters \
        resourceName=${AKS_NAME} \
        dnsPrefix=${AKS_NAME} \
        servicePrincipalClientId=${SPN_CLIENT_ID} \
        servicePrincipalClientSecret=${SPN_PW} \
        workspaceRegion="Canada Central" \
        enableHttpApplicationRouting=true \
        enableOmsAgent=true \
        containerRegistryName=${ACR_NAME}

az extension add --name dev-spaces-preview

az aks use-dev-spaces -n ${AKS_NAME} -g ${AKS_RG}