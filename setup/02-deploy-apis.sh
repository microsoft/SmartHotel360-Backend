#!/bin/bash

registry=
acrName=${ACR_NAME}
imageTag=$(git rev-parse --abbrev-ref HEAD)
dockerOrg="smarthotels"
appName=${SH360_APPNAME}


if [[ "$acrName" == "" ]]
then
  registry=$acrName.azurecr.io
fi


while [ "$1" != "" ]; do
    case $1 in
        -r | --registry)                shift
                                        registry=$1
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
        --release)                      shift
                                        release=$1
                                        ;;
       * )                              echo "Invalid param. Use mandatory -n (or --name)"
                                        echo "Optionals -c (--clean), -r (--registry), -o (--org) or -t (--tag), --release"
                                        exit 1
    esac
    shift
done

pushd ../src/SmartHotel360-Azure-backend/deploy/k8s


if [[ "$acrName" != "" ]]
then
  echo "------------------------------------------------------------"
  echo "Creating the registry $acrName"
  echo "------------------------------------------------------------"
  az acr create -n $acrName -g ${AKS_RG} --admin-enabled --sku Basic
  echo "------------------------------------------------------------"
  echo "Building and pushing to $registry"
  echo "------------------------------------------------------------"
  . build-push.sh $acrName
fi

echo "------------------------------------------------------------"
echo "Deploying the code from registry '$acrName'"
echo "Application Name used: $appName"
echo "Image tag is: $imageTag"
echo "------------------------------------------------------------"

if [[ "$acrName" != "" ]]
then
  . deploy.sh  --registry $registry --release $appName -n $appName -t $imageTag
else
  . deploy.sh  --release $appName -n $appName -t $imageTag
fi


if [[ "${AKS_NAME}" != "" ]]
then

  echo "------------------------------------------------------------"
  echo "Opening the dashboard"
  echo "------------------------------------------------------------"
  az aks browse -n ${AKS_NAME} -g ${AKS_RG}
fi

popd ../../../../setup
