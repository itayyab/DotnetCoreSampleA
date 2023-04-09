param(
  [Parameter(Mandatory=$true)]
  [string]$startIp,
  [Parameter(Mandatory=$true)]
  [string]$endIp,
  [Parameter(Mandatory=$true)]
  [string]$password
)

$location="East US"
$resourceGroup="tayyab-sql-test-rg"
$tag="create-and-configure-database"
$server="tayyab-test-server"
$database="dotnet-store-db"
$login="adminx"

Write-Output "Using resource group $resourceGroup with login: $login, password: $password..."
Write-Output "Creating $resourceGroup in $location..."
az group create --name $resourceGroup --location "$location" --tags $tag

Write-Output "Creating $server in $location..."
az sql server create --name $server --resource-group $resourceGroup --location "$location" --admin-user $login --admin-password $password

Write-Output "Configuring firewall..."
az sql server firewall-rule create --resource-group $resourceGroup --server $server -n AllowMYIp --start-ip-address $startIp --end-ip-address $endIp
az sql server firewall-rule create -g $resourceGroup -s $server -n AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0


Write-Output "Creating $database on $server..."
az sql db create --resource-group $resourceGroup --server $server --name $database --edition GeneralPurpose --family Gen5 --capacity 2 --zone-redundant false 
