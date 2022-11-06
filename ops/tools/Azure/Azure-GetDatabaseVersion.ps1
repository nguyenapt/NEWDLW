<#
.SYNOPSIS
Gets the most recent database version on all detected servers of versioned databases and puts it in a variable.

.DESCRIPTION
Versioned databases are databases for which their name ends with 'yyyyMMdd.#'.

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
[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True, Position=1)] [string]$ResourceGroupName,
    [Parameter(Mandatory=$True, Position=2)] [string]$VariableName,
    [Parameter()] [string]$DatabaseNameFilter = '*'
)

$versionedDbMatch = "(.*?)(\d{8}.\d+)$" # eg nesh.site.dev-20170601.1
$invariantCulture = [System.Globalization.CultureInfo]::InvariantCulture

$versions = Get-AzureRmSqlServer -ResourceGroupName $ResourceGroupName | Get-AzureRmSqlDatabase `
    | where {$_.DatabaseName -ne 'master'} `
    | where {$_.DatabaseName -like $DatabaseNameFilter} `
    | where {$_.DatabaseName -match $versionedDbMatch} `
    | select @{ Name = "Version"; Expression= {[Convert]::ToDecimal("$($Matches[2])", $invariantCulture)} } `
    | measure -Property Version -Maximum

if ($versions) {
    [Environment]::SetEnvironmentVariable($VariableName, $versions[0].Maximum, "Process")
    Write-Host "##vso[task.setvariable variable=$VariableName]$($versions[0].Maximum)"
    Write-Host "Found database version $($versions[0].Maximum) and set it in variable $VariableName."
}
else {
    Write-Error "No versioned databases found."
}