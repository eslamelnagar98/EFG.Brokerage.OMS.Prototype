$serviceName=$args[0]
Start-Service -Name $serviceName
$service = Get-Service -Name $serviceName
while($service.Status -eq "Running")
{
   Write-Host "Service $serviceName is running."
   Start-Sleep -Seconds 5 
   $service = Get-Service -Name $serviceName
}