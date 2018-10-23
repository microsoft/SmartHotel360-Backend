#!/bin/bash

acrName=${ACR_NAME}
registry=
password=${ACR_PASSWORD}
login=${ACR_LOGIN}
secretName="registry-key"

if [[ "$acrName" != "" ]]
then
  registry=$acrName.azurecr.io
fi

while [ "$1" != "" ]; do
    case $1 in
        -r | --registry)                shift
                                        registry=$1
                                        ;;
        -p | --password)                shift
                                        password=$1
                                        ;;
        -u | --user)                    shift
                                        login=$1
                                        ;;
        --secret-name)                  shift
                                        secretName=$1
                                        ;;
        * )                             echo "Invalid param. Use: ./build-secret -u <user> -p <password> [-r <registry>] [--secret-name <k8s-secret-name>]"
                                        exit 1
    esac
    shift
done

if [[ "$registry" == "" ]]
then
  echo "No docker registry specified. Set ACR_NAME env value to ACR name or use -r <fqdn-registry>"
  exit 1
fi

if [[ "$password" == "" ]]
then
  echo "No password specified. Use -p <password> or set ACR_PASSWORD env value"
  exit 1
fi

if [[ "$login" == "" ]]
then
  echo "No docker user specified. Use -u <docker-user> or set ACR_LOGIN env value"
  exit 1
fi

echo "Creating secret named $secretName for storing docker user credentials for registry $registry"

kubectl delete secret $secretName
kubectl create secret docker-registry $secretName --docker-server $registry --docker-username $login --docker-password $password --docker-email not@used.com

