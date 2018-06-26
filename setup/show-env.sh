export AKS_SUB=$aks_sub export AKS_RG=$aks_rg export AKS_NAME=$aks_name export ACR_NAME=$acr_name export AKS_REGION=$aks_region export SPN_CLIENT_ID=$spn_client_id export SPN_PW=$spn_pw export SH360_APPNAME=$sh360_appname

echo "Current environment variables:"
echo "SH306_APPNAME (SH360 Application Name): ${SH360_APPNAME}"
echo "AKS_SUB (Azure subscription): ${AKS_SUB}"
echo "AKS_RG (Resource Group): ${AKS_RG}"
echo "AKS_NAME (AKS Name): ${AKS_NAME}"
echo "ACR_NAME (ACR Name): ${ACR_NAME}"
echo "AKS_REGION (Azure Region): ${AKS_REGION}"
echo "SPN_CLIENT_ID (Client ID): ${SPN_CLIENT_ID}"
echo "SPN_PW (Client Password): ${SPN_PW}"
echo "----------------------------------------------------------"

