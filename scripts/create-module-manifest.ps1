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

function LoadAssemblies {
  param (
    [string]
    $ModuleFolder
  )

  Get-ChildItem -Path $ModuleFolder *.dll `
  | ForEach-Object { [System.Reflection.Assembly]::LoadFrom($_.FullnName) }
}

function GetModuleFunctions {
  param (
    [string]
    $ModuleFolder,

    [string]
    $ModuleName
  )

  $assemblyPath = Join-Path $ModuleFolder "$ModuleName.dll"
  $assembly = [System.Reflection.Assembly]::LoadFrom($assemblyPath)
  $types = $assembly.GetTypes().Where({ [System.Management.Automation.PsCmdlet].IsAssignableFrom($_) })

  $types.GetCustomAttributes([System.Management.Automation.CmdletAttribute], $false).Foreach({ "$($_.VerbName)-$($_.NounName)" })
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

LoadAssemblies

$functionsToExport =
  GetModuleFunctions `
    -ModuleFolder $publishVariables.ModuleFolder `
    -ModuleName $publishVariables.ModuleName

$version =
  GetVersionNumber `
    -MajorVersion $publishVariables.MajorVersion `
    -MinorVersion $publishVariables.MinorVersion `
    -PatchVersion $PatchVersion

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
  -FunctionsToExport $functionsToExport
