#!/bin/bash

registry=
clean=0
imageTag=$(git rev-parse --abbrev-ref HEAD)
release=
dockerOrg="smarthotels"
appName=
dns=""

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
                                        appName=$1
                                        ;;
        --release)                      shift
                                        release=$1
                                        ;;
        -d | --dns)                     shift
                                        dns=$1
                                        ;;
       * )                              echo "Invalid param. Use mandatory -n (or --name)"
                                        echo "Optionals -c (--clean), -r (--registry), -o (--org) or -t (--tag), --release"
                                        exit 1
    esac
    shift
done


if [[ "$dns" == "none" ]]
then
  dns=""
fi

if [[ "$appName" == "" ]]
then
  echo "must provide a name using -n or --name"
  exit 1
fi


echo "Using registry: $registry, tag $imageTag & organization $dockerOrg"
echo "Base name for releases is (empty means random for each release): $release"

echo "Getting the nginx controller ip of the cluster... (this can take a while)"
nginxip=$(kubectl get svc -n kube-system | grep "LoadBalancer" | awk '{print $4}')
echo "DNS used to ingress resources (if any): '$dns' (Use -d <dns> to config  DNS for ingress). nginx-ingress IP value detected is: $nginxip"

if (( clean == 1 ))
then
  echo "cleaning all helm releases from cluster"
  helm ls --short | xargs -L1 helm delete --purge
fi


declare -a infra=("sh360-postgres|postgres" "sh360-sql-data|sql")

for inf in "${infra[@]}"
do 
  IFS='|' read -r -a array <<< "$inf"

  if [[ "$release" != "" ]]
  then
    fullrelease="$release-${array[1]}"
  fi

  echo "Installing infrastructure ${array[0]} (helm release name is $fullrelease (blank means random)"
  helm install ${array[0]} --name=$fullrelease --set appName=$appName -f infrastructure_values.yml 
done

declare -a arr=("sh360-hotels|hotels" "sh360-bookings|bookings" "sh360-config|configuration" "sh360-discounts|discounts" "sh360-notifications|notifications" "sh360-profiles|profiles" "sh360-reviews|reviews" "sh360-suggestions|suggestions" "sh360-tasks|tasks")

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

  if [[ "$release" != "" ]]
  then
    fullrelease="$release-${array[1]}"
  fi

  echo "Installing service ${array[0]} (image $currentImage)"

   helm install ${array[0]} --name=$fullrelease --set image.tag=$imageTag --set image.repository=$currentImage --set appName=$appName --set ingress.enabled=1 --set ingress.hosts={$dns} -f ingress_values.yml -f pull_secrets_conf.yml -f infrastructure_values.yml

done


