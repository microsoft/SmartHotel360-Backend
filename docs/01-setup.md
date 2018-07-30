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
* The [jq](https://stedolan.github.io/jq/) package for bash, which enables jQuery processing. 
* [Helm](https://helm.sh/) and [Draft](https://github.com/Azure/draft) to ease Kubernetes deployment

## Set up a Service Principal 

[Create a service principal](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal?view=azure-cli-latest) and take note of the Application ID and key. The service principal will need to be added to the **Contributor** for the subscription.

If you already have a service principal, you can re-use it, and if you don't and create one for this demo, you can re-use it to create other AKS clusters in the future. 

## Provision the Azure Resources

In this step you'll create all of the Azure resources required by the demo. This consists of an AKS Cluster and an Azure Container Registry (ACR) instance. The AKS Cluster is pre-configured to use Microsoft Operations Management Suite (OMS) and Log Analytics to enable the rich Container Health Dashboard capabilities. 

1. Clone this repository to your development machine. 
1. Install the Azure Dev Spaces **preview** extension for the Azure CLI by entering the following command. 

    ```bash
    az extension add --name dev-spaces-preview
    ```

1. Open a bash terminal. git clone this repository. Then CD into the `setup` folder of this repository. 
1. Some Linux distributions require setting execute permissions on `.sh` files prior to executing them. To be safe, running the command below results in the bash scripts being enabled with execution priveleges. 

    ```bash
    chmod +x ./00-set-vars.sh
    chmod +x ./01-aks-create.sh
    chmod +x ./02-deploy-apis.sh
    chmod +x ./03-deploy-web.sh
    chmod +x ../src/SmartHotel360-Azure-backend/deploy/k8s/build-push.sh
    chmod +x ../src/SmartHotel360-Azure-backend/deploy/k8s/deploy.sh
    ```

1. Run the command below, replacing the parameters with your own values. The script expects following parameters:

    * `-g <resource-group>`: Resource group to use
    * `-s <subscription>`: Azure Subscription to use
    * `-n <name>`: AKS cluster name to be used
    * `-r <name>`: ACR name to be used (just name, not FQDN)
    * `-l <location>`: Location to be used. Defaults to `eastus`
    * `-c <spn-client>`: Service principal app id
    * `-p <spn-pwd>`: Service principal password
    * `-a <name>`: Name of the Sh360 app to be installed in the cluster. Defaults to  `myapp`

    ```bash
    source 00-set-vars.sh -g <resource group> -s <subscription id> -n <cluster name> -r <ACR name> -l eastus -c <service principal app id> -p <service principal password>
    ```

    > **Important Note:** The only regions in which AKS and Azure Dev Spaces are currently supported are Canada East and East US. So when creating a new AKS cluster for this scenario use either **canadaeast** or **eastus** for the **AKS_REGION** variable.

1. Once the script has run, create the Azure resources you'll need by running this script:

    ```bash
    source 01-aks-create.sh
    ```

Now that the AKS cluster has been created we can publish the SmartHotel360 microservice source code into it. 

## Deploy the SmartHotel360 Backend APIs

In this segment you'll build the images containing the SmartHotel360 back-end APIs and publish them into ACR, from where they'll be pulled and pushed into AKS when you do your deployment. We've scripted the complex areas of this to streamline the setup process, but you're encouraged to look in the `.sh` files to see (or improve upon) what's happening. 

1. CD into the `setup` directory (if not already there) and run this command:

    ```bash
    ./02-deploy-apis.sh --httpRouting
    ```

    The script will take some time to execute, but when it is complete the `az aks browse` command will be executed and the Kubernetes dashboard will open in your browser.  Details on this script can be found [here](deploy/02-deploy-apis.md), so you can customize creation if you desire. The script above should be enough once the environment variables are set in the previous step. 

1. When the dashboard opens (you may need to hit refresh as it may 404 at first), some of the objects in the cluster may not be fully ready. Hit refresh until these are all green and at 100%. 

    ![Waiting until green](../media/still-yellow.png)

1. Within a few minutes the cluster will show 100% for all of the objects in it. 

    ![All ready](../media/all-green.png)

Congratulations! You've deployed the APIs. You're 75% of the way there, now, and all that remains is to deploy the public web site. This is a good opportunity for a much-earned break!

## Deploy the Public Web App

Now that the back-end APIs are in place the public web app can be pushed into the cluster, too. The spa app makes calls to the APIs running in the cluster and answers HTTP requests at the ingress URL you used earlier.

1. The end-to-end setup script makes use of some of the `export` environment variables you set earlier, so make sure they're still set by using the `echo` command to make sure they're still set. If you don't see values when you `echo` these environment variables, re-run `setup/00-set-vars.sh`. 

1. CD into the `setup` directory if you're not already there, and execute the command below. 

    ```bash
    ./03-deploy-web.sh --httpRouting
    ```

    The command may take a few minutes to complete. Details on how to customize the deployment using the script's parameters are [here](deploy/03-deploy-web.md), but the above script should be good enough for you to deploy the web app. 

1. Once the command completes, execute the command below to see all of the ingresses in the AKS cluster. 

    ```bash
    kubectl get ingresses
    ```

    You'll see a list of service ingresses that have been deployed into the cluster, and each should have a public IP address assigned to it.

    ![Ingresses](../media/ingresses.png)

1. One of the ingresses has the suffix **sh360-web** - that's the public web site ingress (the prefix is dynamically generated by Kubernetes). Copy the IP address from the terminal window.

    ![Copy ingress](../media/copy-fqdn.png)

1. Open a browser window and paste the URL into a browser and the public site should appear. 

    ![Public web](../media/public-web.png)

1. The final step is to enable your cluster with **Azure Dev Spaces**, which enables rich debugging capabilities within Visual Studio and Visual Studio Code. Run this script to setup Azure Dev Spaces into the cluster. 

    ```bash
    az aks use-dev-spaces -n ${AKS_NAME} -g ${AKS_RG} --space default --update --yes
    ```

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
