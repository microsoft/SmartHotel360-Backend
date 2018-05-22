cd ../src/SmartHotel360-public-web/

echo "------------------------------------------------------------"
echo "Building and pushing container"
echo "------------------------------------------------------------"
cd SmartHotel360.PublicWeb
docker build -t ${ACR_NAME}.azurecr.io/smarthotels/publicweb .

az acr login -n ${ACR_NAME}
docker push ${ACR_NAME}.azurecr.io/smarthotels/publicweb
cd ../manifests

echo "------------------------------------------------------------"
echo "Applying the deployment to the cluster"
echo "------------------------------------------------------------"
kubectl apply -f deployment.yaml
kubectl set image deployments/publicweb publicweb=${ACR_NAME}.azurecr.io/smarthotels/publicweb
kubectl rollout resume deployments/publicweb
kubectl apply -f service.yaml
kubectl apply -f ingress.yaml

cd ../../../setup