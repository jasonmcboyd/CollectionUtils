[CmdletBinding()]
param (
  [Parameter(Mandatory = $true)]
  [string]
  $Version
)

$ErrorActionPreference = 'Stop'
Set-strictMode -Version Latest

function GetModuleFiles {
  param (
    [string]
    $ModuleFolder,

    [string]
    $ModuleName
  )

  Get-ChildItem -Path $ModuleFolder *.dll | Select-Object -ExpandProperty Name

  "$ModuleName.psd1"
}

. $PSScriptRoot/variables.ps1

$fileList =
  GetModuleFiles `
    -ModuleFolder $publishVariables.ModuleFolder `
    -ModuleName $publishVariables.ModuleName

# Parse version from tag (e.g., "v0.0.61-alpha" -> version "0.0.61", prerelease "alpha")
$versionString = $Version -replace '^v', ''
$parts = $versionString -split '-', 2
$version = [System.Version]::new($parts[0])
$prerelease = if ($parts.Length -gt 1) { $parts[1] } else { $null }

$functionsToExport = @(
  'ConvertFrom-DataTable'
  'ConvertFrom-SmartCsv'
  'Import-SmartExcel'
  'Convert-Property'
  'ConvertTo-Hashtable'
  'Import-SmartCsv'
  'Join-Collection'
  'Test-Collection'
)

$manifestParams = @{
  Path              = $publishVariables.ManifestPath
  Author            = 'Jason Boyd'
  ModuleVersion     = $version
  Guid              = $publishVariables.Guid
  RootModule        = "$($publishVariables.ModuleName).dll"
  FileList          = $fileList
  LicenseUri        = 'https://raw.githubusercontent.com/jasonmcboyd/CollectionUtils/main/LICENSE'
  ProjectUri        = 'https://github.com/jasonmcboyd/CollectionUtils'
  FunctionsToExport = $functionsToExport
  Description       = 'A collection of utilities for working with collections in PowerShell'
  Tags              = 'Join','InnerJoin','OuterJoin','LeftJoin','RightJoin','CrossJoin','ZipJoin','DataTable','HashTable','Partition'
}

if ($prerelease) {
  $manifestParams['Prerelease'] = $prerelease
}

New-ModuleManifest @manifestParams
