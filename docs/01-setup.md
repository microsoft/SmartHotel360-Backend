# SmartHotel360 AKS Demo Setup

All of the back-end systems run inside of Docker containers. During the installation phase you will notice errors if you haven't set your Docker configuration to use 4 GB of memory. Changing this is simple within the Docker configuration dialog. Just set the memory higher and restart Docker.

> Note: The code and deployment process are being updated to use Helm and more Kubernetes-conventional tactics, and minor changes may occur prior to the general availability of Azure Dev Spaces. Watch this repository for updates, as they'll be coming by Summer 2018. 

## Prerequisites 

* Bash command line, which can be accomplished natively on Mac or Linux or using [Windows Subsystem for Linux](https://docs.microsoft.com/en-us/windows/wsl/install-win10) on Windows.
* [Docker](http://www.docker.com) to build the containers. 
* [Visual Studio 2017 Preview](https://www.visualstudio.com/vs/preview/) with the ASP.NET Web workload installed. 
* [Azure Dev Spaces Extension for Visual Studio](https://docs.microsoft.com/en-us/azure/dev-spaces/get-started-netcore-visualstudio#get-the-visual-studio-tools). 
* [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)
* [Kubernetes CLI](https://kubernetes.io/docs/tasks/tools/install-kubectl/)

## Set up a Service Principal 

[Create a service principal](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal?view=azure-cli-latest) and take note of the Application ID and key. The service principal will need to be added to the **Contributor** for the subscription.

If you already have a service principal, you can re-use it, and if you don't and create one for this demo, you can re-use it to create other AKS clusters in the future. 

## Provision the Azure Resources

In this step you'll create all of the Azure resources required by the demo. This consists of an AKS Cluster and an Azure Container Registry (ACR) instance. The AKS Cluster is pre-configured to use Microsoft Operations Management Suite (OMS) and Log Analytics to enable the rich Container Health Dashboard capabilities. 

1. Install the Azure Dev Spaces **preview** extension for the Azure CLI by entering the following command. 

    ```bash
    az extension add --name dev-spaces-preview
    ```

1. Open the `setup\00-set-vars.sh` file in a text editor. 

1. Set the variables in the script below (the `exports`). 

    **Important Note:** The only regions in which AKS and Azure Dev Spaces are currently supported are Canada East and Eaat US. So when creating a new AKS cluster for this scenario use either **canadaeast** or **eastus** for the **AKS_REGION** variable.

    ```bash
    export AKS_SUB=<azure subscription id> \ 
    export AKS_RG=<resource group name> \ 
    export AKS_NAME=<AKS cluster name> \ 
    export ACR_NAME=<Azure Container Registry name> \ 
    export AKS_REGION=eastus \ 
    export SPN_CLIENT_ID=<service principal app id> \ 
    export SPN_PW=<service principal password>
    ```

    Save the file once you're happy with your edits. If you open a new instance of your terminal window or close before ending the process, you can re-run `setup/00-set-vars.sh` to reset the environment variables the other scripts will use. 

1. Open a bash terminal. CD into the `setup` folder of this repository. 
1. Some Linux distributions require setting execute permissions on `.sh` files prior to executing them. To be safe, running the command below results in the bash scripts being enabled with execution priveleges. 

    ```bash
    chmod +x ./00-set-vars.sh
    chmod +x ./01-aks-create.sh
    chmod +x ./02-deploy-apis.sh
    chmod +x ./03-deploy-web.sh
    chmod +x ../src/SmartHotel360-Azure-backend/deploy/k8s/build-push.sh
    chmod +x ../src/SmartHotel360-Azure-backend/deploy/k8s/deploy.sh
    ```

1. Run the command below. 

    ```bash
    source 00-set-vars.sh
    source 01-aks-create.sh
    ```

1. Once the cluster is created, take note of the URL value for the `HTTPApplicationRoutingZoneName` property in the response JSON payload. Copy this URL, as it will be used later when deploying the microservices. 

    ```json
    "outputs": {
      "aks": {
        "type": "Object",
        "value": {
          "addonProfiles": {
            "httpApplicationRouting": {
              "config": {
                "HTTPApplicationRoutingZoneName": "<guid>.eastus.aksapp.io"
              },

    ```

Now that the AKS cluster has been created we can publish the SmartHotel360 microservice source code into it. 

## Deploy the SmartHotel360 Backend APIs

In this segment you'll build the images containing the SmartHotel360 back-end APIs and publish them into ACR, from where they'll be pulled and pushed into AKS when you do your deployment. We've scripted the complex areas of this to streamline the setup process, but you're encouraged to look in the `.sh` files to see (or improve upon) what's happening. 

> Note: Very soon, the repo will be signififcantly updated to make use of Helm as a deployment strategy. For now the scripts are mostly in bash and make use of `kubectl` to interface with the cluster. 


1. The end-to-end setup script makes use of some of the `export` environment variables you set earlier, so make sure they're still set by using the `echo` command to make sure they're still set. If you don't see values when you `echo` these environment variables, re-run `setup/00-set-vars.sh`. 

    ```bash
    echo ${AKS_NAME}
    echo ${AKS_RG}
    echo ${ACR_NAME}
    echo ${AKS_SUB}
    ```
1. CD into the `setup` directory (if not already there) and run this command:

    ```bash
    source 02-deploy-apis.sh
    ```

    The script will take some time to execute, but when it is complete the `az aks browse` command will be executed and the Kubernetes dashboard will open in your browser.

1. When the dashboard opens (you may need to hit refresh as it may 404 at first), some of the objects in the cluster may not be fully ready. Hit refresh until these are all green and at 100%. 

    ![Waiting until green](../media/still-yellow.png)

1. Within a few minutes the cluster will show 100% for all of the objects in it. 

    ![All ready](../media/all-green.png)

Congratulations! You've deployed the APIs. You're 75% of the way there, now, and all that remains is to deploy the public web site. This is a good opportunity for a much-earned break!

## Set up Ingress

In order to route traffic to the various APIs within the AKS cluster, you'll need to set up a front door, or "ingress." 

> Note: Eventually this step will be deprecated in lieu of the integrated ingress features of AKS. We're in the process of improving the deployment by using Helm charts, and when we do that we'll start using AKS's integrated ingress features. 

1. If you forgot to note the `HTTPApplicationRoutingZoneName` property earlier, execute the command below to get the JSON representation of your cluster. 

    ```bash
    az resource show --api-version 2018-03-31 --id /subscriptions/${AKS_SUB}/resourceGroups/${AKS_RG}/providers/Microsoft.ContainerService/managedClusters/${AKS_NAME}
    ```

1. You'll see the `HTTPApplicationRoutingZoneName` property in the JSON in the terminal window. Copy this value as it will be needed in the next step. 

    ```json
    "properties": {
        "addonProfiles": {
        "httpApplicationRouting": {
            "config": {
            "HTTPApplicationRoutingZoneName": "de44228e-2c3e-4bd8-98df-cdc6e54e272a.eastus.aksapp.io"
            }
    ```

1. Open up the `ingress.yaml` file and see line 14, which has the `host` property set as follows:

    ```yaml
    spec:
      rules:
      - host: sh360.<guid>.<region>.aksapp.io
          http:
    ```

1. Replace the `host` property with the `sh360.` followed by value you copied earlier. 

    ```yaml
    spec:
      rules:
      - host: sh360.de44228e-2c3e-4bd8-98df-cdc6e54e272a.eastus.aksapp.io
          http:
    ```

1. Execute the command below to set the AKS cluster ingress. 

    ```bash
    kubectl apply -f ingress.yaml
    ```

1. Open up the `src/SmartHotel360-public-web/manifests/ingress.yaml` file and see line 9, which has the `host` property set as follows:

    ```yaml
    spec:
      rules:
      - host: sh360.<guid>.<region>.aksapp.io
        http:
    ```

1. Replace the `host` property with the `sh360.` followed by value you copied earlier. 

    ```yaml
    spec:
      rules:
      - host: sh360.de44228e-2c3e-4bd8-98df-cdc6e54e272a.eastus.aksapp.io
        http:
    ```

## Deploy the Public Web App

Now that the back-end APIs are in place the public web app can be pushed into the cluster, too. The spa app makes calls to the APIs running in the cluster and answers HTTP requests at the ingress URL you used earlier.

1. The end-to-end setup script makes use of some of the `export` environment variables you set earlier, so make sure they're still set by using the `echo` command to make sure they're still set. If you don't see values when you `echo` these environment variables, re-run `setup/00-set-vars.sh`. 

1. CD into the `setup` directory if you're not already there, and execute the command below. 

    ```bash
    source 03-deploy-web.sh
    ```

    The command may take a few minutes to complete. 

1. Once the command completes, paste the string you applied to the `host` property in the `ingress.yaml` files (the `sh360.<guid>.<region>.aksapp.io` string) into a browser, and you'll see the SmartHotel360 public web site load up. 

    ![Public web](../media/public-web.png)

## Save the Queries

There are three queries provided in the [`queries`](../queries) folder of this repository:

* CPU chart of the entire cluster over time
* Error log containing "0 results found" log entry
* Bar chart over time of the error log containing "0 results found" log entry

To make it easy to run these queries during a demo, paste them in the Log Analytics Query Explorer and click the **Save** button, then give the query a name and category. 

![Save the query](../media/save-queries.png)

Then they're readily available in the **Saved Queries** folder in the Query Explorer. 

![Running the query](../media/saved-queries-running.png)

## Success!

Now that the setup is complete, you can read the [Demo Script](02-script.md) to see how to execute the demo. Or, if you want to preload the cluster you just created with data, learn how the [preloading script can help you](03-preload.md). 
