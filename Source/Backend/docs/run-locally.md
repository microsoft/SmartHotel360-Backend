# Run services locally

> **Note**: To run services you need [Docker CE](https://www.docker.com/community-edition) installed and running. All steps must be done from `/src` folder.

## From public Docker images

All docker images are published in Docker Hub, under the `smarthotels` organization. **Must use the tag `public`. Other tags are no guaranted to work**. Just type:

```
docker-compose -f docker-compose-tagged.yml -f docker-compose.override.yml up sql-data tasks-data reviews-data
``` 

This will start all databases. APIs needs that database available on startup so wait for all log messages to finish (it could take about a minute) and then open another command prompt, navigate to the same `/src` folder, and type:

```
docker-compose -f docker-compose-tagged.yml -f docker-compose.override.yml up
```

This will start all other APIs.

>**Note** Those images are configured to use the public Azure environment provided by Microsoft.

## From your own images

The first step to run all services is build the docker images by typing `docker-compose build`. This will use Docker to compile all code and generate the Docker images (you don't need to have installed any SDK). The process could take some time.

Once images are built, you can run all services locally by typing first:

```
docker-compose up sql-data tasks-data reviews-data
```

This will start all databases. APIs needs that database available on startup so wait for all log messages to finish (it could take about a minute) and then open another command prompt, navigate to the same `/src` folder, and type:

```
docker-compose up 
```

This will start all other containers (the APIs). This process could take some minutes to complete.

## Checking images are running

Finally check all the containers are running, by typing `docker ps`. Output should be like:

```
deb73ca3ce47        smarthotels/tasks                      "/bin/sh -c 'exec ja…"   About a minute ago   Up About a minute   0.0.0.0:6104->8080/tcp   src_tasks-api_1
5a3109ad8a6d        smarthotels/profiles                   "dotnet SmartHotel.S…"   About a minute ago   Up About a minute   0.0.0.0:6108->80/tcp     src_profiles-api_1
9fa5f874dc9c        smarthotels/suggestions                "/bin/sh -c 'npm sta…"   About a minute ago   Up About a minute   0.0.0.0:6102->80/tcp     src_suggestions-api_1
f6caab02c203        smarthotels/notifications              "dotnet SmartHotel.S…"   About a minute ago   Up About a minute   0.0.0.0:6105->80/tcp     src_notifications-api_1
a8d6e52864e4        smarthotels/bookings                   "dotnet SmartHotel.S…"   About a minute ago   Up About a minute   0.0.0.0:6100->80/tcp     src_bookings-api_1
af9ec51dd966        smarthotels/discounts                  "dotnet SmartHotel.S…"   About a minute ago   Up About a minute   0.0.0.0:6107->80/tcp     src_discounts-api_1
a6056d0a49b4        smarthotels/configuration              "dotnet SmartHotel.S…"   About a minute ago   Up About a minute   0.0.0.0:6103->80/tcp     src_configuration-api_1
d9d2a0e823fd        smarthotels/reviews                    "/bin/sh -c 'exec ja…"   About a minute ago   Up About a minute   0.0.0.0:6106->8080/tcp   src_reviews-api_1
cac8091b3c6b        smarthotels/hotels                     "dotnet SmartHotel.S…"   About a minute ago   Up About a minute   0.0.0.0:6101->80/tcp     src_hotels-api_1
85cc291963f8        postgres:10.1                          "docker-entrypoint.s…"   About a minute ago   Up About a minute   5432/tcp                 src_tasks-data_1
583501a43770        postgres:10.1                          "docker-entrypoint.s…"   About a minute ago   Up About a minute   5432/tcp                 src_reviews-data_1
c24a39cb3d01        microsoft/mssql-server-linux:2017-GA   "/bin/sh -c /opt/mss…"   About a minute ago   Up About a minute   0.0.0.0:6433->1433/tcp   src_sql-data_1
```

> **Note** if you are running the images from DockerHub the name of the images will have the suffix `:public`. This is expected.

All APIs (except `smarthotels/configuration` and `smarthotels/suggestions`) have Swagger UI enabled. To access the Swagger UI of one API just open one browser and type `http://localhost:<port>` where `<port>` is the mapped port of the API (i.e. 6104 for tasks api, 6108 for profiles api).

![swagger ui](./swagger-ui.png)

