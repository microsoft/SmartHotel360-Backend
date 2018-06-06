#!/bin/bash

registry=
clean=0
imageTag=$(git rev-parse --abbrev-ref HEAD)
release=
dockerOrg="smarthotels"


while [ "$1" != "" ]; do
    case $1 in
        -c | --clean)                   clean=1
                                        ;;
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
                                        release=$1
                                        ;;
       * )                              echo "Invalid param. Use -c (--clean), -r (--registry), -o (--org) or -t (--tag)"
                                        exit 1
    esac
    shift
done

echo "Installing release $release"
echo "Using registry: $registry, tag $imageTag & organization $dockerOrg"

if (( clean == 1 ))
then
  echo "cleaning all helm releases from cluster"
  helm ls --short | xargs -L1 helm delete
fi


declare -a infra=("sh360-postgres" "sh360-sql-data")

for inf in "${infra[@]}"
do
  echo "Installing infrastructure $inf"
  helm install $svc --name=$release
done

declare -a arr=("sh360-hotels|hotels" "sh360-bookings|2" "sh360-config|3" "sh360-discounts|4" "sh360-notifications|5" "sh360-profiles|6" "sh360-reviews|7" "sh360-suggestions|8" "sh360-tasks|9")

for svc in "${arr[@]}"
do
  IFS='|' read -r -a array <<< "$svc"
  currentImage=${array[1]}
  
  if [[ "$dockerOrg" != "" ]]
  then
    currentImage="$dockerOrg/$currentImage"
  fi

  if [[ "$registry" != "" ]]
  then
    currentImage="$registry/$currentImage"
  fi

  echo "Installing service ${array[0]} (image $currentImage)"
  helm install $svc --name=$release --set image.tag=$imageTag --set image.repository=$currentImage
done


