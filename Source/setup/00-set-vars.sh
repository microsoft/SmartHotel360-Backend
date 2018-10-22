#!/bin/bash

aks_sub=${AKS_SUB}
aks_rg=${AKS_RG}
aks_name=${AKS_NAME}
acr_name=${ACR_NAME}
aks_region=${AKS_REGION:-eastus}
spn_client_id=${SPN_CLIENT_ID}
spn_pw=${SPN_PW}
sh360_appname=${SH360_APPNAME:-myapp}
help_shown=0

usage() {
  if (( $help_shown == 0 ))
  then
    help_shown=1
    echo "Usage: source 00-set-vars.sh  -p value (or --parameter value). Parameters are:"
    echo "   -g (--resource-group): Your resource group name"
    echo "   -s (--subscripion): Subscription id"
    echo "   -n (--name): Cluster name"
    echo "   -r (--registry): ACR name"
    echo "   -l (--location): Location (defaults to eastus)"
    echo "   -c (--spn-client): Service principal app id"
    echo "   -p (--spn-pwd): Service principal pwd"
    echo "   -a (--app-name): Name of the sh360 application to install (defaults to myapp)"
  fi
}


if [ "$1" == "" ]
then
  usage
else
  while [ "$1" != "" ]; do
    case $1 in
      -g | --resoure-group)   shift
                              aks_rg=$1
                              ;;
      -s | --subscription)    shift
                              aks_sub=$1
                              ;;
      -n | --name)            shift
                              aks_name=$1
                              ;;
      -r | --registry)        shift
                              acr_name=$1
                              ;;
      -l | --location)        shift
                              aks_region=$1
                              ;;
      -c | --spn-client)      shift
                              spn_client_id=$1
                              ;;
      -p | --spn-pwd)         shift
                              spn_pw=$1
                              ;;
      -h | --help)            shift
                              usage
                              ;;
      -a | --app-name)        shift
                              sh360_appname=$1
                              ;;
      * )                     usage
      esac
    shift
  done
fi

export AKS_SUB=$aks_sub
export AKS_RG=$aks_rg
export AKS_NAME=$aks_name
export ACR_NAME=$acr_name
export AKS_REGION=$aks_region
export SPN_CLIENT_ID=$spn_client_id
export SPN_PW=$spn_pw
export SH360_APPNAME=$sh360_appname
. show-env.sh
