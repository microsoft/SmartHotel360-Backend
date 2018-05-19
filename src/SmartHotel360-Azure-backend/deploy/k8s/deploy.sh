#!/bin/bash
set -x
registry=$1
dockerOrg=${3:-smarthotels}
imageTag=${2:-$(git rev-parse --abbrev-ref HEAD)}

echo "Removing existing services & deployments.."
kubectl delete -f deployments.yaml
kubectl delete -f services.yaml
kubectl delete configmap config-files
kubectl delete configmap externalcfg
kubectl delete configmap discovery-file

kubectl create configmap config-files --from-file=nginx-conf=nginx.conf
kubectl label configmap config-files app=smarthotels

echo "Creating empty discovery service file from $discoveryServiceFile. This is not an error!"
kubectl create configmap discovery-file --from-file=custom.json=empty.json

echo "Deploying WebAPIs"
kubectl create -f services.yaml

echo "Deploying configuration from conf_all.yml"
kubectl create -f conf_all.yml

echo "Creating deployments on k8s..."
kubectl create -f deployments.yaml

# update deployments with the correct image (with tag and/or registry)
echo "Update Image containers to use prefix \"$registry/$dockerOrg\" and tag \"$imageTag\""
kubectl set image deployments/hotels hotels=${registry}/${dockerOrg}/hotels:$imageTag
kubectl set image deployments/bookings bookings=${registry}/${dockerOrg}/bookings:$imageTag
kubectl set image deployments/suggestions suggestions=${registry}/${dockerOrg}/suggestions:$imageTag
kubectl set image deployments/tasks tasks=${registry}/${dockerOrg}/tasks:$imageTag
kubectl set image deployments/config config=${registry}/${dockerOrg}/configuration:$imageTag
kubectl set image deployments/notifications notifications=${registry}/${dockerOrg}/notifications:$imageTag
kubectl set image deployments/reviews reviews=${registry}/${dockerOrg}/reviews:$imageTag
kubectl set image deployments/discounts discounts=${registry}/${dockerOrg}/discounts:$imageTag
kubectl set image deployments/profiles profiles=${registry}/${dockerOrg}/profiles:$imageTag

echo "Execute rollout..."
kubectl rollout resume deployments/hotels
kubectl rollout resume deployments/bookings
kubectl rollout resume deployments/suggestions
kubectl rollout resume deployments/config
kubectl rollout resume deployments/tasks
kubectl rollout resume deployments/notifications
kubectl rollout resume deployments/reviews
kubectl rollout resume deployments/discounts
kubectl rollout resume deployments/profiles
