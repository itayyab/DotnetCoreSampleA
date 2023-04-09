param(
  [Parameter(Mandatory=$true)]  
  [string]$conn
)
$resourceGroup="tayyab-k8s-test-rg"
$cluster="tayyab-cluster"

az group create --name $resourceGroup --location eastus

Write-Output "Sleeping for 45 seconds"
Start-Sleep -Seconds 45


az aks create -g $resourceGroup -n $cluster --enable-managed-identity --node-count 1 --enable-addons monitoring --generate-ssh-keys

Write-Output "Sleeping for 5 seconds"
Start-Sleep -Seconds 5

az aks get-credentials --resource-group $resourceGroup --name $cluster

Write-Output "Sleeping for 5 seconds"
Start-Sleep -Seconds 5

$content = Get-Content k8s-deployment.yml | ForEach-Object { $_ -replace '\{\{DBCONNECTION\}\}', $conn }

Set-Content -Path k8s-deployment-new.yml -Value $content

kubectl apply -f k8s-deployment-new.yml