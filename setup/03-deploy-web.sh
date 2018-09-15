#!/bin/bash

registry=
acrName=${ACR_NAME}
aksName="${AKS_NAME}" 
aksRg="${AKS_RG}"
imageTag=$(git rev-parse --abbrev-ref HEAD)
dockerOrg="smarthotels"
appName=${SH360_APPNAME}
customLogin=""
customPassword=""
aksName=${AKS_NAME}
aksRg=${AKS_RG}
clean=0
useAcr=1
release=
push=1
build=1
dns=""
httpRouting=0

while [ "$1" != "" ]; do
    case $1 in
        -r | --registry)                shift
                                        useAcr=0
                                        registry=$1
                                        ;;
        -n | --name)                    shift
                                        appName=$1
                                        ;;
        --aks-name)                     shift
                                        aksName=$1
                                        ;;
        --aks-rg)                       shift
                                        aksRg=$1
                                        ;;
        --release)                      shift
                                        release=$1
                                        ;;
        -a | --acr)                     shift
                                        acrName=$1
                                        ;;
        -t | --tag)                     shift
                                        imageTag=$1
                                        ;;
        -o | --org)                     shift
                                        dockerOrg=$1
                                        ;;
       --httpRouting)                  httpRouting=1
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
        --clean)                        clean=1
                                        ;;
        --no-push)                      push=0
                                        ;;
        --no-build)                     build=0
                                        ;;
        -d | --dns)                     shift
                                        dns=$1
                                        ;;
        * )                             echo "Invalid param. Use mandatory -n (or --name)"
                                        echo "Optionals -c (--clean), -r (--registry), -o (--org) or -t (--tag), .-a (--acr) or --release"
                                        exit 1
    esac
    shift
done


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

if [[ "$appName" == "" ]]
then
  echo "must provide a name using -n or --name"
  . show-env.sh  
  exit 1
fi

if (( $httpRouting == 1 ))
then
  echo "Use of --httpRouting overrides -d"   
  echo "Autodetecting DNS of $aksName in $aksRg"   
  dns=$(az resource show --api-version 2018-03-31 --id /subscriptions/${AKS_SUB}/resourceGroups/${AKS_RG}/providers/Microsoft.ContainerService/managedClusters/${AKS_NAME} --query properties.addonProfiles.httpApplicationRouting.config.HTTPApplicationRoutingZoneName | tr -d '"')
  echo "DNS detected is: $dns"   
  if [[ "$dns" == "" ]]   
  then     
    echo "No DNS could be auto-detected. Ensure cluster is AKS with HTTP Routing Enabled & AZ CLI is properly configured"     
    exit 1   
  fi   
  dns="$appName.$dns" 
fi

if (( clean == 1 ))
then
  echo "cleaning all helm releases from cluster"
  helm ls --short | xargs -L1 helm delete --purge
fi

if [[ "$acrName" != "" && "$registry" == "" ]]
then
  registry=$acrName.azurecr.io
fi

if [[ "$registry" == "" ]]
then
  echo "Push and build disabled because no registry has been set. Use -a (or ACR_NAME env value) to use an ACR or use -r <fqdn> to use any other docker registry"
  build=0
  push=0
fi

pushd ../src/SmartHotel360-public-web/

if (( $build ))
then
  echo "------------------------------------------------------------"
  echo "Building public web and tagging as $imageTag"
  echo "------------------------------------------------------------"
  pushd SmartHotel360.PublicWeb
  docker build -t $registry/smarthotels/publicweb:$imageTag .
  popd
fi

if (( $push ))
then
  echo "-------------------------------------------------------------"
  echo "Pushing image to $registry"
  echo "-------------------------------------------------------------"
  if (( $useAcr ))
  then
    echo "Signing into ACR"
    az acr login -n ${ACR_NAME} -g $aksRg
  else
    if [[ "$customLogin" == "" || "$customPassword" == "" ]]
    then
      echo "Error: If used -r you must pass custom registry user (--user) and password (--pwd)."
      exit 1
      docker login -p $customPassword -u $customLogin $registry
    fi
  fi
  docker push $registry/smarthotels/publicweb:$imageTag
fi

pushd deploy/k8s

echo "------------------------------------------------------------"
echo "Deploying chart to cluster"
echo "Ingress dns is $dns"
echo "------------------------------------------------------------"

currentImage="publicweb"

if [[ "$dockerOrg" != "" ]]
  then
  currentImage="$dockerOrg/$currentImage"
fi

if [[ "$registry" != "" ]]
then
echo "------------------------------------------------------------"
echo "$registry/$currentImage"
echo "------------------------------------------------------------"
  currentImage="$registry/$currentImage"
fi

if [[ "$release" == "" ]]
  then
  release=$appName
fi

fullrelease="$release-publicweb"

echo "Installing publicweb ($currentImage)"

echo "helm install sh360-web --name=$fullrelease --set image.tag=$imageTag --set image.repository=$currentImage --set appName=$appName --set ingress.enabled=1 --set ingress.hosts={$dns} -f ingress_values.yml -f pull_secrets_conf.yml -f infrastructure_values.yml -f web_values.yml"
helm install sh360-web --name=$fullrelease --set image.tag=$imageTag --set image.repository=$currentImage --set appName=$appName --set ingress.enabled=1 --set ingress.hosts={$dns} -f ingress_values.yml -f pull_secrets_conf.yml -f infrastructure_values.yml -f web_values.yml

popd
