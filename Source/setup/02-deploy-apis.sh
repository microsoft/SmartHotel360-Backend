#!/bin/bash

registry=
acrName="${ACR_NAME}"
imageTag=$(git rev-parse --abbrev-ref HEAD)
dockerOrg="smarthotels"
appName="${SH360_APPNAME}"
customLogin=""
customPassword=""
aksName="${AKS_NAME}"
aksRg="${AKS_RG}"
createAcr=1
clean=1
dns="none"
httpRouting=0

while [ "$1" != "" ]; do
    case $1 in
        -r | --registry)                shift
                                        registry=$1
                                        ;;
        --aks-rg)                       shift
                                        aksRg=$1
                                        ;;
        --aks-name)                     shift
                                        aksName=$1
                                        ;;
        -a | --acr)                     shift
                                        acrName=$1
                                        ;;
        --acr-no-create)                createAcr=0
                                        ;; 
        -t | --tag)                     shift
                                        imageTag=$1
                                        ;;
        -o | --org)                     shift
                                        dockerOrg=$1
                                        ;;
        -n | --name)                    shift
                                        appName=$1
                                        ;;
        --user)                         shift
                                        customLogin=$1
                                        ;;
        --password)                     customPassword=$1
                                        shift
                                        ;;
        --release)                      shift
                                        release=$1
                                        ;;
        --no-clean)                     clean=0
                                        ;;
        -d | --dns)                     shift
                                        dns=$1
                                        ;;
        --httpRouting)                  httpRouting=1
                                        ;;
       * )                              echo "Invalid param. Use mandatory -n (or --name)"
                                        echo "Optionals -c (--clean), -r (--registry), -o (--org) or -t (--tag), .-a (--acr) or --release"
                                        exit 1
    esac
    shift
done

function validateParams {
  if (( $httpRouting == 1 ))
  then
    if [[ "$aksName" == "" ]]
    then
      echo "No cluster is specified. Please use --aks-name or set the AKS_NAME env value."
      . show-env.sh
      exit 1
    fi
    if [[ "$aksRg" == "" ]]
    then
      echo "No resource group is specified. Please use --aks-rg or set the AKS_RG env value".
      . show-env.sh
      exit 1
    fi
  fi

  if (( $createAcr ))
  then
    if [[ "$aksRg" == "" ]]
    then
      echo "No resource group is specified. Please use --aks-rg or set the AKS_RG env value".
      . show-env.sh
      exit 1
    fi
  fi
}

validateParams

if (( $httpRouting == 1 ))
then
  echo "Use of --httpRouting overrides -d"
  echo "Autodetecting DNS of $aksName in $aksRg"
  dns=$(az aks show -n ${aksName} -g ${AKS_RG} --query addonProfiles.httpApplicationRouting.config.HTTPApplicationRoutingZoneName | tr -d '"')
  echo "DNS detected is: $dns"
  if [[ "$dns" == "" ]]
  then
    echo "No DNS could be auto-detected. Ensure cluster is AKS with HTTP Routing Enabled & AZ CLI is properly configured"
    exit 1
  fi
  dns="$appName.$dns"
fi

if [[ "$acrName" != "" && "$registry" == "" ]]
then
  registry=$acrName.azurecr.io
fi

pushd ../Backend/deploy/k8s


if [[ "$acrName" != "" ]]
then
  if (( $createAcr ))
  then
    echo "------------------------------------------------------------"
    echo "Creating the registry $acrName" in rg $aksRg
    echo "------------------------------------------------------------"
    az acr create -n $acrName -g $aksRg --admin-enabled true --sku Basic
  else
    echo "Skipping ACR creation for $acrName (--acr-no-create used)"
  fi
  echo "------------------------------------------------------------"
  echo "Building and pushing to ACR $acrName"
  echo "------------------------------------------------------------"
  ./build-push.sh -a $acrName -t $imageTag

  echo "Creating secret on k8s. Retrieving login & pwd of ACR $acrName"

  acrPassword=$(az acr credential show -n $acrName | jq .passwords[0].value | tr -d '"')
  acrLogin=$(az acr credential show -n $acrName | jq .username | tr -d '"')

  ./deploy-secret.sh -r $registry -p $acrPassword -u $acrLogin
fi

if [[ "$acrName" == "" && "$registry" != "" ]]
then
  echo "------------------------------------------------------------"
  echo "Building and pushing to custom registry $acrName"
  echo "------------------------------------------------------------"
  ./build-push.sh -r $registry -t $imageTag --user $customLogin --password $customPassword
 
  echo "You have specified a custom registry (not ACR). Login and pwd can't be retrieved automatically."
  if [[ "$customLogin" == "" || "$customPassword" == "" ]] 
  then
    echo "Error: Must pass custom registry user (--user) and password (--pwd)."
    exit 1
  fi
  echo "Creating secret on k8s for custom docker registry (not ACR)"
  ./deploy-secret.sh -r $registry -p $customLogin -u $customPassword
fi

if (( $clean ))
then
  echo "Cleaning all previous releases"
  ./clean.sh
  else
  echo "Cleaning skipped (--no-clean used)"
fi

echo "------------------------------------------------------------"
echo "Deploying the code from registry '$registry'"
echo "Application Name used: $appName"
echo "Image tag is: $imageTag"
echo "Ingess DNS: $dns"
echo "------------------------------------------------------------"

if [[ "$registry" != "" ]]
then
  ./deploy.sh  --registry $registry --release $appName -n $appName -t $imageTag -d $dns
else
  ./deploy.sh  --release $appName -n $appName -t $imageTag -d $dns
fi

popd 

if [[ "$aksName" != "" ]]
then

  echo "------------------------------------------------------------"
  echo "Opening the dashboard"
  echo "------------------------------------------------------------"
  az aks browse -n $aksName -g $aksRg
fi
