#!/bin/bash

registry=
acrName=${ACR_NAME}
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
appName=publicwebsite
push=1
build=1
dns="none"

while [ "$1" != "" ]; do
    case $1 in
        -r | --registry)                shift
                                        useAcr=0
                                        registry=$1
                                        ;;
        -n | --name)                    shift
                                        appName=$1
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
        --clean)                        clean=1
                                        ;;
        --no-push)                      push=0
                                        ;;
        --no-build)                     build=0i
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



if [[ "$appName" == "" ]]
then
  echo "must provide a name using -n or --name"
  exit 1
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
    az acr login -n ${ACR_NAME}
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

currentImage="publicweb:$imageTag"

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

if [[ "$release" != "" ]]
  then
  fullrelease="$release-publicweb"
fi

echo "Installing publicweb ($currentImage)"

echo "helm install sh360-web --name=$fullrelease --set image.tag=$imageTag --set image.repository=$currentImage --set appName=$appName --set ingress.enabled=1 --set ingress.hosts={$dns} -f ingress_values.yml -f pull_secrets_conf.yml"
helm install sh360-web --name=$fullrelease --set image.tag=$imageTag --set image.repository=$currentImage --set appName=$appName --set ingress.enabled=1 --set ingress.hosts={$dns} -f ingress_values.yml -f pull_secrets_conf.yml

popd
