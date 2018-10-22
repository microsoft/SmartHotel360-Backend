#!/bin/bash


acrName=${ACR_NAME}
aksRg="${AKS_RG}"
dockerOrg=smarthotels
imageTag=$(git rev-parse --abbrev-ref HEAD)
customLogin=
customPassword=
useAcr=1
push=1
build=1

while [ "$1" != "" ]; do
  case $1 in
      -a | --acr)               shift
                                acrName=$1
                                ;;
      -o | --org)               shift
                                dockerOrg=$1
                                ;;
      -t | --tag)               shift
                                imageTag=$1
                                ;;
      -r | --registry)          shift
                                useAcr=0
                                registry=$1
                                ;;
      --user)                   shift;
                                customLogin=$1
                                ;;
      --password)               shift
                                customPassword=$1
                                ;;
      --no-push)                push=0
                                ;;
      --no-build)               build=0
                                ;;
      * )                       echo "Invalid param. Use -a <acr-name>, -o <docker-org>, -t <tag> or -r <fqdn-registry>"
                                exit 1

  esac
  shift
done

if [[ "$acrName" != "" && "$registry" == "" ]]
then
  registry=$acrName.azurecr.io
fi

if [[ "$registry" == "" ]]
then
  echo "Must specify registry using either -a <acr-name>, or setting the ACR_NAME env value or using -r <fqdn-registry> for a non-ACR registry)"
  exit 1
fi

if (( $build ))
then
  echo "------------------------------------------------------------"
  echo "Building Docker images tagged with public tag"
  echo "------------------------------------------------------------"
  export TAG=$imageTag
  docker-compose -p .. -f ../../src/docker-compose.yml -f ../../src/docker-compose-tagged.yml build
else
  echo "docker build skipped (--no-build used)"
fi

if (( $useAcr ))
then

  echo "------------------------------------------------------------"
  echo "Logging into ACR $acrName"
  echo "------------------------------------------------------------"
  az acr login -n $acrName -g $aksRg
else
  echo "------------------------------------------------------------"
  echo "Logging into custom registry $registry"
  echo "------------------------------------------------------------"
    if [[ "$customLogin" == "" || "$customPassword" == "" ]]
    then
      echo "Error: Must pass custom registry user (--user) and password (--pwd)."
      exit 1
    fi
    docker login -p $customPassword -u $customLogin $registry
fi

if (( $push ))
then

  echo "----------------------------------------------------------------------------------"
  echo "Retagging public images to $imageTag and pushing images to $registry/$dockerOrg..."
  echo "----------------------------------------------------------------------------------"
  services="bookings hotels suggestions tasks configuration notifications reviews discounts profiles"
  for service in $services; do
    imageFqdn=$registry/${dockerOrg}/${service}
    docker tag smarthotels/${service}:public ${imageFqdn}:$imageTag
    docker push ${imageFqdn}:$imageTag
  done
else
  echo "Push skipped (--no-push used)"
fi
