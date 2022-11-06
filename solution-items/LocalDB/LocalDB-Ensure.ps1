# SqlLocalDb runs as 64bit process, if your processes is running as 32-bit mode under local serivce user, you must use syswow64 folders to have the it correctly working.
# Check the userprofile folder and see if it runs under a system 32 folder, if so change some environment variables.
$userProfile = $Env:USERPROFILE
if($userProfile.ToLower() -eq "c:\Windows\system32\config\systemprofile")
{	
	$Env:USERPROFILE = "C:\Windows\syswow64\config\systemprofile"
	Write-Host "Changed Env:USERPROFILE to ${Env:USERPROFILE}"
	$Env:LOCALAPPDATA = "C:\Windows\syswow64\config\systemprofile\AppData\Local"
	Write-Host "Changed Env:LOCALAPPDATA to ${Env:LOCALAPPDATA}"
	$restoreUserSettings = $TRUE
}

&SqlLocalDB.exe create Projects -s

if($restoreUserSettings)
{	
	$Env:USERPROFILE = "C:\Windows\system32\config\systemprofile"
	Write-Host "Changed Env:USERPROFILE to ${Env:USERPROFILE}"
	$Env:LOCALAPPDATA = "C:\Windows\system32\config\systemprofile\AppData\Local"
	Write-Host "Changed Env:LOCALAPPDATA to ${Env:LOCALAPPDATA}"	
}

# NOTE: will not work if you are running powershell in 64-bit mode with local service user. You must run the powershell 32-bit mode variant in the case of local service user.

$ConnectionString = "Data Source=(LocalDB)\Projects;Initial Catalog=master"
$Command = @"
	EXEC sp_configure 'clr enabled', 1;
	RECONFIGURE
"@

$SqlConnection = New-Object System.Data.SqlClient.SqlConnection
$SqlConnection.ConnectionString = $ConnectionString
$SqlConnection.Open()

$SqlCmd = New-Object System.Data.SqlClient.SqlCommand
$SqlCmd.CommandText = $Command
$SqlCmd.Connection = $SqlConnection
$SqlCmd.ExecuteNonQuery()

$SqlConnection.Dispose()