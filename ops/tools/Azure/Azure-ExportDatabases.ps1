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
	[Parameter(Mandatory=$True, Position=7)] [string]$ServerAdmin,
	[Parameter(Mandatory=$True, Position=8)] [string]$ServerPassword,
	[Parameter()] [string]$DatabaseNameFilter = '*'
)

function Select-NewestDatabaseFromGroup {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$True, ValueFromPipeline=$True)]
        [object[]]$groups
    )
    Process {
        foreach ($group in $groups) {
            $newest = $null
            foreach ($item in $group.Group) {
                if ($newest -eq $null -or $newest.Version -lt $item.Version) {
                    $newest = $item
                }
            }
            $newest
        }
    }
}

$versionedDbMatch = "(.*?)(\d{8}.\d+)$" # eg nesh.site.dev-20170601.1
$invariantCulture = [System.Globalization.CultureInfo]::InvariantCulture

# Get all databases to copy.
$databases = Get-AzureRmSqlServer -ResourceGroupName $ResourceGroupName | Get-AzureRmSqlDatabase `
    | where {$_.DatabaseName -ne 'master'} `
    | where {$_.DatabaseName -like $DatabaseNameFilter} `
    | where {$_.DatabaseName -match $versionedDbMatch} `
    | select DatabaseName, ServerName, @{ Name = "Name"; Expression = {$Matches[1]} }, @{ Name = "Version"; Expression= {[convert]::ToDecimal("$($Matches[2])", $invariantCulture)} } `
    | group {$_.Name} `
    | Select-NewestDatabaseFromGroup
Write-Host "Found $(($databases | measure).Count) versioned databases to copy on server $SqlServerName."

$securePassword = ConvertTo-SecureString -String $ServerPassword -AsPlainText -Force
$creds = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $ServerAdmin, $securePassword

workflow exportDatabases {
	param (
		$databasesToExport,
		$storageName,
		$storageContainerName,
		$storageKey,
		$resourceGroupName,
		$creds,
		$profilePath
	)
		
		# Copy databases.
	foreach -Parallel ($db in $databasesToExport) {
		Import-AzureRmContext -Path $profilePath
		
		$bacpacFilename = $db.DatabaseName + ".bacpac"

		$baseStorageUri = "https://$storageName.blob.core.windows.net/$storageContainerName/"
		$bacpacUri = $baseStorageUri + $bacpacFilename

		$exportRequest = New-AzureRmSqlDatabaseExport -ResourceGroupName $resourceGroupName -ServerName $db.ServerName `
		  -DatabaseName $db.DatabaseName -StorageKeytype StorageAccessKey -StorageKey $storageKey -StorageUri $bacpacUri `
		  -AdministratorLogin $creds.UserName -AdministratorLoginPassword $creds.Password

		# block untill export is done
		$exportStatus = Get-AzureRmSqlDatabaseImportExportStatus -OperationStatusLink $exportRequest.OperationStatusLink
		Write-Progress -Activity "Exporting"
		while ($exportStatus.Status -eq "InProgress")
		{
			$exportStatus = Get-AzureRmSqlDatabaseImportExportStatus -OperationStatusLink $exportRequest.OperationStatusLink
			Write-Progress -Activity "Exporting"
			Start-Sleep -s 10
		}
		# Write-Progress -Activity $exportStatus.Status
	}
}

$profilePath = Join-Path (Get-Item -Path ".\" -Verbose).FullName profile.json
Save-AzureRmContext -Path $profilePath -Force

exportDatabases -databasesToExport $databases -storageName $StorageName -storageContainerName $StorageContainerName -storageKey $StorageKey -resourceGroupName $ResourceGroupName -creds $creds -profilePath $profilePath

Write-Host "Done"