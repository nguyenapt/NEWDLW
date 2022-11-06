<#
.SYNOPSIS
Trigger an sql database export command to create bacpac file into a specific storage account and blocks untill export is done.

.DESCRIPTION
Bacpac filename contains version database, name ends with 'yyyyMMdd.#'.

.PARAMETER ResourceGroupName
Name of the resource group that contains the databases.

.PARAMETER VariableName
VariableName to set the found version to. Also support VSTS Build vNext and Release Management.

.PARAMETER DatabaseNameFilter
Optional filter to apply to database names.

.NOTES
Script expects an authenticated powershell session to the Azure target subscription:
    Login-AzureRmAccount
    Set-AzureRmContext -SubscriptionName 'target subscription name'
#>

# Script based on sample: https://docs.microsoft.com/en-us/azure/sql-database/sql-database-export

[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True, Position=1)] [string]$ResourceGroupName,
	[Parameter(Mandatory=$True, Position=2)] [string]$StorageName,
	[Parameter(Mandatory=$True, Position=3)] [string]$StorageKey,
	[Parameter(Mandatory=$True, Position=4)] [string]$StorageContainerName,
    [Parameter(Mandatory=$True, Position=5)] [string]$DatabaseName,
	[Parameter(Mandatory=$True, Position=6)] [string]$ServerName,
	[Parameter(Mandatory=$True, Position=7)] [string]$ServerAdmin,
	[Parameter(Mandatory=$True, Position=8)] [string]$ServerPassword
)

$securePassword = ConvertTo-SecureString -String $ServerPassword -AsPlainText -Force
$creds = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $ServerAdmin, $securePassword

$bacpacFilename = $DatabaseName + ".bacpac"

$baseStorageUri = "https://$StorageName.blob.core.windows.net/$StorageContainerName/"
$bacpacUri = $baseStorageUri + $bacpacFilename

try
{   
    $exportRequest = New-AzureRmSqlDatabaseExport -ResourceGroupName $ResourceGroupName -ServerName $ServerName `
	  -DatabaseName $DatabaseName -StorageKeytype StorageAccessKey -StorageKey $StorageKey -StorageUri $bacpacUri `
	  -AdministratorLogin $creds.UserName -AdministratorLoginPassword $creds.Password
	  
	# block untill export is done
	$exportStatus = Get-AzureRmSqlDatabaseImportExportStatus -OperationStatusLink $exportRequest.OperationStatusLink
	Write-Host "Exporting" -nonewline
	while ($exportStatus.Status -eq "InProgress")
	{
		$exportStatus = Get-AzureRmSqlDatabaseImportExportStatus -OperationStatusLink $exportRequest.OperationStatusLink
		Write-Host "." -nonewline
		Start-Sleep -s 10
	}
	Write-Host $exportStatus.Status
}
catch
{
    Write-Warning "Export bacpac $bacpacUri failed, skipping export."
	Write-Warning "Exception: $_.Exception.Message"
}