$ErrorActionPreference = 'Stop'
Set-strictMode -Version Latest

$publishVariables = @{
  Guid           = '28db5ff8-be2b-4bb2-bdd9-c112ced131b8'
  MajorVersion   = 0
  MinorVersion   = 0
  RepositoryRoot = (Split-Path $PSScriptRoot -Parent)
}

$publishVariables['ModuleName']     = Split-Path $publishVariables['RepositoryRoot'] -Leaf
$publishVariables['SolutionFolder'] = Join-Path $publishVariables['RepositoryRoot'] 'src'
$publishVariables['ProjectFolder']  = Join-Path $publishVariables['RepositoryRoot'] "src/$($publishVariables['ModuleName'])"
$publishVariables['PublishFolder']  = Join-Path $publishVariables['RepositoryRoot'] 'publish'
$publishVariables['ModuleFolder']   = Join-Path $publishVariables['PublishFolder'] $publishVariables['ModuleName']
$publishVariables['ManifestPath']   = Join-Path $publishVariables['ModuleFolder'] "$($publishVariables['ModuleName']).psd1"

$publishVariables = [PSCustomObject]$publishVariables
