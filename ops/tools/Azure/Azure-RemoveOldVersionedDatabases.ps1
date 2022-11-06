<#
.SYNOPSIS
Removes old database versions of versioned databases.

.DESCRIPTION
Versioned databases are databases for which their name ends with 'yyyyMMdd'.

.PARAMETER SqlResourceGroupName
Name of the resource group that contains the databases.

.PARAMETER SqlServerName
Name of the Sql server that hosts the databases.

.PARAMETER DatabaseNameFilter
Optional filter to apply to database names to limit copies of databases.

.NOTES
Script expects an authenticated powershell session to the Azure target subscription:
    Login-AzureRmAccount
    Set-AzureRmContext -SubscriptionName 'target subscription name'
#>
[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True, Position=1)] [string]$ResourceGroupName,
    [Parameter()] [string]$DatabaseNameFilter = '*'
)

function Select-OldDatabasesFromGroup {
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
            foreach ($item in $group.Group) {
                if ($item -ne $newest) {
                    $item
                }
            }
        }
    }
}

$versionedDbMatch = "(.*?)(\d{8}.\d+)$" # eg nesh.site.dev-20170601.1
$versionRegex = "(\d{8}).(\d+)$"
$invariantCulture = [System.Globalization.CultureInfo]::InvariantCulture

function ToComparableVersion([string]$version) {
	if (-Not ($version -match $versionRegex)) {
		Write-Host "Version '$($version)' is not correct."
	}
	
	$paddedDecimal = $Matches[2].PadLeft(2, '0')
	return [convert]::ToDecimal("$($Matches[1]).$($paddedDecimal)", $invariantCulture)
}

# Get all databases to remove.
$databases = Get-AzureRmSqlServer -ResourceGroupName $ResourceGroupName | Get-AzureRmSqlDatabase `
    | where {$_.DatabaseName -ne 'master'} `
    | where {$_.DatabaseName -like $DatabaseNameFilter} `
    | where {$_.DatabaseName -match $versionedDbMatch} `
	| select DatabaseName, ServerName, @{ Name = "Name"; Expression = {$Matches[1]} }, @{ Name = "Version"; Expression= {ToComparableVersion($Matches[2])} } `
    | group {$_.Name} `
    | Select-OldDatabasesFromGroup
Write-Host "Found $(($databases | measure).Count) versioned databases to remove from server $SqlServerName."

# Remove databases.
foreach ($db in $databases) {
    Write-Host "Removing database $($db.DatabaseName) from server $($db.ServerName)"
    $existing = Remove-AzureRmSqlDatabase -ResourceGroupName $ResourceGroupName -ServerName $($db.ServerName) -DatabaseName $($db.DatabaseName) -Force
}
Write-Host "Done"