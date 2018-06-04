#!/bin/bash

cd ../src/SmartHotel360-Azure-backend/deploy/k8s

echo "------------------------------------------------------------"
echo "Creating the registry ${ACR_NAME}"
echo "------------------------------------------------------------"
az acr create -n ${ACR_NAME} -g ${AKS_RG} --admin-enabled --sku Basic

echo "------------------------------------------------------------"
echo "Applying PostgreSQL databases"
echo "------------------------------------------------------------"
kubectl apply -f postgres.yaml

echo "------------------------------------------------------------"
echo "Applying SQL Server databases"
echo "------------------------------------------------------------"
kubectl apply -f sql-data.yaml

echo "------------------------------------------------------------"
echo "Building and pushing to ACR"
echo "------------------------------------------------------------"
source build-push.sh ${ACR_NAME}

echo "------------------------------------------------------------"
echo "Deploying the code"
echo "------------------------------------------------------------"
source deploy.sh ${ACR_NAME}.azurecr.io

echo "------------------------------------------------------------"
echo "Opening the dashboard"
echo "------------------------------------------------------------"
az aks browse -n ${AKS_NAME} -g ${AKS_RG}

cd ../../../../setup