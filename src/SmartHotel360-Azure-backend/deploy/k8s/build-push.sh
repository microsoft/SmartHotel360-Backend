#!/bin/bash
set -ex

registry=$1
dockerOrg=${3:-smarthotels}
imageTag=${2:-$(git rev-parse --abbrev-ref HEAD)}
echo "Docker image Tag: $imageTag"

echo "------------------------------------------------------------"
echo "Building Docker images tagged with $imageTag"
echo "------------------------------------------------------------"
export TAG=$imageTag
docker-compose -p .. -f ../../src/docker-compose.yml -f ../../src/docker-compose-tagged.yml build

echo "------------------------------------------------------------"
echo "Logging into the registry ${ACR_NAME}"
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
