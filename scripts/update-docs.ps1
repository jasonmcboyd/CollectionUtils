$ErrorActionPreference = 'Stop'
Set-strictMode -Version Latest

# Requires platyPS module be installed
#
# Install-Module platyPS

$csprojPath = "$PSScriptRoot/../src/CollectionUtils/CollectionUtils.csproj"
$targetFramework = ([xml](Get-Content $csprojPath)).Project.PropertyGroup.TargetFramework

Import-Module $PSScriptRoot/../src/CollectionUtils/bin/Debug/$targetFramework/CollectionUtils.dll

New-MarkdownHelp -Module CollectionUtils -OutputFolder $PSScriptRoot/../docs/ -Force
