$ErrorActionPreference = 'Stop'
Set-strictMode -Version Latest

# Requires platyPS module be installed
#
# Install-Module platyPS

Import-Module $PSScriptRoot/../src/CollectionUtils/bin/Debug/net6.0/CollectionUtils.dll

New-MarkdownHelp -Module CollectionUtils -OutputFolder $PSScriptRoot/../docs/ -Force
