[CmdletBinding()]
param (
  [Parameter(Mandatory = $true)]
  [int]
  $PatchVersion
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

function GetVersionNumber {
  param (
    [int]
    $MajorVersion,

    [int]
    $MinorVersion,

    [int]
    $PatchVersion
  )

  [System.Version]::new($MajorVersion, $MinorVersion, $PatchVersion)
}

. $PSScriptRoot/variables.ps1

$fileList =
  GetModuleFiles `
    -ModuleFolder $publishVariables.ModuleFolder `
    -ModuleName $publishVariables.ModuleName

$version =
  GetVersionNumber `
    -MajorVersion $publishVariables.MajorVersion `
    -MinorVersion $publishVariables.MinorVersion `
    -PatchVersion $PatchVersion

$functionsToExport = @(
  'ConvertFrom-DataTable'
  'ConvertTo-Hashtable'
  'Join-Collection'
  'Test-Collection'
)

New-ModuleManifest `
  -Path $publishVariables.ManifestPath `
  -Author 'Jason Boyd' `
  -ModuleVersion $version `
  -Prerelease 'alpha' `
  -Guid $publishVariables.Guid `
  -RootModule "$($publishVariables.ModuleName).dll" `
  -FileList $fileList `
  -LicenseUri 'https://raw.githubusercontent.com/jasonmcboyd/CollectionUtils/main/LICENSE' `
  -ProjectUri 'https://github.com/jasonmcboyd/CollectionUtils' `
  -FunctionsToExport $functionsToExport `
  -Description 'A collection of utilities for working with collections in PowerShell' `
  -Tags 'Join','InnerJoin','OuterJoin','LeftJoin','RightJoin','CrossJoin','ZipJoin','DataTable','HashTable','Partition'
