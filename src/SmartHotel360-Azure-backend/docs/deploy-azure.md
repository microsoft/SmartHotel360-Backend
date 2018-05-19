# Creating all Azure resources

Once the kubernetes cluster is created, you need to create all remaining Azure infrastructure (databases, storages and so on). In the `/deploy/k8s/arm` folder there is one ARM script to create all items. You can use the `deploy.cmd` file to deploy the Azure resources in a new resource group by just typing:

```
deploy azuredeploy <resource-group> -c <location>
```

i.e. to deploy in a resource group called `my-new-rg` and located in `eastus` you can type:

```
deploy azuredeploy my-new-rg -c eastus
```

This will create a set of resources like following ones:

![azure resources](./azure-rg.png)

## Other topics

* Read how [run in azure](./run-azure.md)
* Read how [create Kubernetes cluster](../deploy/k8s/readme.md)
* Read how [deploy microservices in Kubernetes](../deploy/k8s/deploy.md)