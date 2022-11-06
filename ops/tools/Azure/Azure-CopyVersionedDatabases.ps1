<#
.SYNOPSIS
Creates new database versions by copying the most recent versioned databases.

.DESCRIPTION
Versioned databases are databases for which their name ends with 'yyyyMMdd'.
The script will copy the most recent version of all versioned databases and will assign them the new supplied version.

When database should already exists, it will be skipped.

.PARAMETER ResourceGroupName
Name of the resource group that contains the databases.

.PARAMETER SqlServerName
Name of the Sql server that hosts the databases.

.PARAMETER NewVersion
New date version for the databases. (yyyyMMdd) 

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
    [Parameter(Mandatory=$True, Position=2)] [string]$NewVersion,    
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

# Validate arguments.
if (("TestDb." + $NewVersion) -notmatch $versionedDbMatch) {
    Write-Error "NewVersion parameter should be a date with format: yyyyMMdd"
    return;
}

# Get all databases to copy.
$databases = Get-AzureRmSqlServer -ResourceGroupName $ResourceGroupName | Get-AzureRmSqlDatabase `
    | where {$_.DatabaseName -ne 'master'} `
    | where {$_.DatabaseName -like $DatabaseNameFilter} `
    | where {$_.DatabaseName -match $versionedDbMatch} `
    | select DatabaseName, ServerName, @{ Name = "Name"; Expression = {$Matches[1]} }, @{ Name = "Version"; Expression= {[convert]::ToDecimal("$($Matches[2])", $invariantCulture)} } `
    | group {$_.Name} `
    | Select-NewestDatabaseFromGroup
Write-Host "Found $(($databases | measure).Count) versioned databases to copy on server $SqlServerName."

# Copy databases.
foreach ($db in $databases) {
    $newName = ($db.DatabaseName -replace $versionedDbMatch, "`$1")  + $NewVersion
    Write-Host "Copying database $($db.DatabaseName) to $newName"
	$serverName = $db.ServerName
	Write-Host "Copying from and to server $serverName"
	
    $existing = Get-AzureRmSqlDatabase -ResourceGroupName $ResourceGroupName -ServerName $serverName -DatabaseName $newName `
        -ErrorAction SilentlyContinue # Ignore error when not exists.
    if ($existing) {
        Write-Warning "Database with name $newName alread exists, skipping copy."
        continue
    }
	
	Write-Host "Copy database."
    $copy = New-AzureRmSqlDatabaseCopy -ResourceGroupName $ResourceGroupName -ServerName $serverName -DatabaseName $db.DatabaseName -CopyDatabaseName $newName `
        -WarningAction SilentlyContinue # Ignore tag parameter usability warning.
}
Write-Host "Done"