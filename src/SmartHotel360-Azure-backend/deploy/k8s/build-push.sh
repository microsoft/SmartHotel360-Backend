#!/bin/bash


registry=${1:-${ACR_NAME}}
dockerOrg=${3:-smarthotels}
imageTag=${2:-$(git rev-parse --abbrev-ref HEAD)}

if [[ "$registry" == "" ]]
then
  echo "Must specify registry or set ACR_NAME env variable"
  exit 1
fi

echo "------------------------------------------------------------"
echo "Building Docker images tagged with $imageTag"
echo "------------------------------------------------------------"
export TAG=$imageTag
docker-compose -p .. -f ../../src/docker-compose.yml -f ../../src/docker-compose-tagged.yml build

echo "------------------------------------------------------------"
echo "Logging into the registry $registry
echo "------------------------------------------------------------"
az acr login -n $registry

echo "------------------------------------------------------------"
echo "Pushing images to $registry.azurecr.io/$dockerOrg..."
echo "------------------------------------------------------------"
services="bookings hotels suggestions tasks configuration notifications reviews discounts profiles"
for service in $services; do
  imageFqdn=$registry.azurecr.io/${dockerOrg}/${service}
  docker tag smarthotels/${service}:public ${imageFqdn}:$imageTag
  docker push ${imageFqdn}:$imageTag
done
