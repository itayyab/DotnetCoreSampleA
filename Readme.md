# Dotnet Core Online Store App
Sample online store app created using Dotnet Core and Angular.

# Technology stack:
* .Net 6.
* Angular 8.

## Docker
To build the docker image use:
```
docker build -t dot-net-core-store-app --progress=plain . 
```
To publish the image use:
```
docker push dot-net-core-store-app:latest
```
To run the application using docker use: 
```
docker run -ti -p 8080:80 -e ConnectionStrings__DefaultConnection="Server=tcp:127.0.0.1,1433;Database=DotnetCoreSampleA_DB;User ID=xxxxxx;Password=xxxxxxx;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;"  dot-net-core-store-app
```
The application can be access using localhost:8080.

## Azure SQL Database:
* To provision SQL database on Azure use the [azsqlserver.ps1](scripts/azsqlserver.ps1) script. The scripts asks for IP that can be whitelisted in the firewall. It also asks for the password to be used for SQL Server.
* To deploy the Application on Azure k8s cluster use [azk8s.ps1](scripts/azk8s.ps1) script. The script will require you to provide the connection string for your database connection.
* To clean up the Azure resources use: [destroy.ps1](scripts/destroy.ps1).

	