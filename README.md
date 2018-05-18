# SmartHotel360
During **Connect(); 2017** event this year we presented beautiful app demos using Xamarin and many features of Azure. For //build/ 2018's keynote, we updated some components of the back-end API code to support **Azure Kubernetes Service (AKS)**. This repository contains the setup instructions and sample code needed to repeat the [AKS and Azure Dev Spaces demo](https://www.youtube.com/watch?v=rd0Rd8w3FZ0&feature=youtu.be&t=8890) from //build/ 2018. 

# SmartHotel360 Repos
For this reference app scenario, we built several consumer and line-of-business apps and an Azure backend. You can find all SmartHotel360 repos in the following locations:

* [SmartHotel360 ](https://github.com/Microsoft/SmartHotel360)
* [Backend Services V2 (AKS Optimized)](https://github.com/Microsoft/SmartHotel360-AKS-backend)
* [Backend Services](https://github.com/Microsoft/SmartHotel360-Azure-backend)
* [Public Website](https://github.com/Microsoft/SmartHotel360-public-web)
* [Mobile Apps](https://github.com/Microsoft/SmartHotel360-mobile-desktop-apps)
* [Sentiment Analysis](https://github.com/Microsoft/SmartHotel360-Sentiment-Analysis-App)
* [Migrating Internal apps to Azure](https://github.com/Microsoft/SmartHotel360-internal-booking-apps)

# SmartHotel360 - Backend Services

Welcome to the SmartHotel360 AKS repository. Here you'll find everything you need to run the backend services locally and/or deploy them in a AKS cluster.

## Getting Started

SmartHotel360 uses a **microservice oriented** architecture implemented using containers. There are various services developed in different technologies: .NET Core 2, Java, and Node.js. These services use different data stores like PostgreSQL and SQL Server. The documentation is divided into the docs:

1. [Demo Setup](docs/01-setup.md) - Create the Azure resources and demo environment setup, and deploy the SmartHotel360 services to the AKS Cluster.
1. [Demo Script](docs/02-script.md) - Steps and video example for running the demo.

End-to-end setup takes about an hour provided you have all of the development enviroment prerequisites met. 

## Storyteller's Advice
Here's an important note if you'd like to run the demo. 
The AKS Container Health Dashboard is highlighted in this demo with a series of charts that show CPU data across the cluster and a detailed snapshot of how the APIs are running with extended log data over a period of time. If you want to mimic the demo, you'll want to run the [data-loading script](docs/03-preload.md) over a span of 12 hours. Otherwise the data represented in the chart will be quite minimal. 

# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
