## SmartHotel360 Microservices on Azure Kubernetes Service

During the [Build 2018](http://buildwindows.com/) keynote, Scott Hanselman (with help from Scott Guthrie) showed developers the new features available in Azure Kubernetes Service and Azure Dev Spaces. Today, AKS reaches general availability, and to help you learn how to deploy microservices written in any framework to AKS we've updated the [SmartHotel360](http://www.smarthotel360.com) back-end microservices source code and deployment process to optimize it for AKS. You can clone, fork, or download [the new demo at GitHub](https://github.com/Microsoft/SmartHotel360-AKS-DevSpaces-Demo), where the [original SmartHotel360 repositories](https://github.com/Microsoft/SmartHotel360) are also still available. 

The [original back-end demo]() has been updated with these improvements:

* Updated demo script
* [Significantly simplified setup process](https://github.com/Microsoft/SmartHotel360-AKS-DevSpaces-Demo/blob/master/docs/01-setup.md), written in bash, so it can be easily executed on Linux, Mac, or in Windows using [WSL](https://docs.microsoft.com/en-us/windows/wsl/about). 
* [Helm](https://helm.sh/) charts for each service
* Sample queries for use in with AKS Log Search
* A [preloader script](https://github.com/Microsoft/SmartHotel360-AKS-DevSpaces-Demo/blob/master/docs/03-preload.md) that can be used to generate log/CPU data
* The Hotels API has a bug that one must fix and debug during the demo

## Web App Changes

The [SmartHotel360 public web site](https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/SmartHotel360-Demo-App-Web-Site) was originally written to demonstrate the features that make Azure App Service the best place in the cloud to host [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/getting-started?view=aspnetcore-2.1&tabs=macos) applications, with amazing diagnostics, deployment, and devops features. 

AKS is a great place to host ASP.NET Core applications, too, so to give you great examples of **both scenarios**, we've moved the public web site into the AKS cluster for this sample. If you're investigating the variety of options for hosting your ASP.NET Core apps in Azure, you'll have the [original App Service-focused version](https://github.com/Microsoft/SmartHotel360-public-web) of the demo source code, and you'll learn from [the new demo repository]((https://github.com/Microsoft/SmartHotel360-AKS-DevSpaces-Demo)) how to publish an ASP.NET Core app into AKS. 

## Azure Dev Spaces

App Service and the rich Visual Studio family of IDEs have given developers a variety of live debugging features for years. Azure Dev Spaces enable developers to get up and running, debugging their code directly in the AKS cluster, without the need to configure any environments locally or to need to replicate an AKS cluster and everything in it. Azure Dev Spaces enables a more productive development and debugging process by enabling in-container debugging, within a cluster. 

The Helm charts included in the updated sample enable not only a streamlined deployment experience, but also the Dev Spaces functionality. 

## Summary

Azure Kubernetes Service (AKS) brings so many amazing features for developers. The container health dashboard, deep log search features enabling you to really see how your code's executing in the cluster, and IDE/debugger integration that makes it possible for you to edit and debug code live in the cluster without impacting production or teammate code all make AKS the greatest experience for building apps with Kubernetes. 

We hope this demo is useful in your process of learning how to publish microservices to AKS and to make the most use of the amazing portal and debugging features. As with all of the SmartHotel360 repositories, these are open and we encourage pull requests. If you experience any issues setting it up, send us an issue in GitHub and we'll resolve it quickly. 

Thanks! We hope you enjoy this AKS-optimized demonstration. 