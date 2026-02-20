---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# Test-Collection

## SYNOPSIS

Tests whether any or all items in a collection satisfy a predicate script block.

## SYNTAX

### InputObject|PredicateScript|All
```
Test-Collection [-PredicateScript] <ScriptBlock> [-InputObject] <PSObject[]> [-All]
 [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

### InputObject|PredicateScript|Any
```
Test-Collection [-PredicateScript] <ScriptBlock> [-InputObject] <PSObject[]> [-Any]
 [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

## DESCRIPTION

`Test-Collection` applies a predicate script block to each item in a collection and returns a single boolean result. You must specify exactly one of `-Any` or `-All` to choose the quantifier.

- `-Any`: Returns `$true` as soon as any item satisfies the predicate. Returns `$false` if no items match. Short-circuits — stops processing after the first match.
- `-All`: Returns `$false` as soon as any item fails the predicate. Returns `$true` if all items match. Short-circuits — stops processing after the first non-match.

Both quantifiers handle empty collections consistently with LINQ semantics: `-Any` returns `$false` and `-All` returns `$true` for an empty input.

The predicate script block receives each item as `$_` and should return exactly one boolean value. A warning is written if the script block returns a value that is not a single boolean.

## EXAMPLES

### Example 1: Test whether any process is using more than 100 MB of memory

```powershell
$result = Get-Process | Test-Collection { $_.WorkingSet64 -gt 100MB } -Any

if ($result) {
    Write-Host 'At least one process is using more than 100 MB.'
}
```

Processing stops as soon as the first matching process is found, making this efficient for large collections.

### Example 2: Test whether all files in a directory are under 1 MB

```powershell
$allSmall = Get-ChildItem -File | Test-Collection { $_.Length -lt 1MB } -All

if (-not $allSmall) {
    Write-Warning 'One or more files exceed 1 MB.'
}
```

Returns `$false` immediately on the first file that is 1 MB or larger; does not continue reading the remaining files.

### Example 3: Test a list of numbers for any negative values

```powershell
$numbers = -3, 0, 7, 14

$hasNegative = $numbers | Test-Collection { $_ -lt 0 } -Any  # Returns $true
```

Works on any collection that can be piped, including plain arrays of primitive values.

### Example 4: Verify all services in a group are running

```powershell
$services = 'wuauserv', 'bits', 'winmgmt' | ForEach-Object { Get-Service $_ }

$allRunning = $services | Test-Collection { $_.Status -eq 'Running' } -All

if (-not $allRunning) {
    Write-Warning 'One or more required services are not running.'
}
```

Use `-All` to enforce a condition across every member of a set before proceeding.

### Example 5: Observe empty collection behavior

```powershell
$empty = @()

$empty | Test-Collection { $_ -gt 0 } -Any   # Returns $false — nothing matched
$empty | Test-Collection { $_ -gt 0 } -All   # Returns $true  — nothing violated the condition
```

Empty input follows standard quantifier semantics: existential (`-Any`) is false, universal (`-All`) is true.

## PARAMETERS

### -All

Tests whether every item in the collection satisfies the predicate. Returns `$false` and stops processing as soon as any item does not satisfy the predicate. Returns `$true` if all items satisfy the predicate or if the collection is empty.

```yaml
Type: SwitchParameter
Parameter Sets: InputObject|PredicateScript|All
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Any

Tests whether at least one item in the collection satisfies the predicate. Returns `$true` and stops processing as soon as any item satisfies the predicate. Returns `$false` if no items satisfy the predicate or if the collection is empty.

```yaml
Type: SwitchParameter
Parameter Sets: InputObject|PredicateScript|Any
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InputObject

The collection of objects to test. Accepts pipeline input. Each object is passed to the predicate script block as `$_`.

```yaml
Type: PSObject[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -PredicateScript

A script block that receives each item as `$_` and must return exactly one boolean value (`$true` or `$false`). A warning is written if the script block returns a value that is not a single boolean. The script block is evaluated once per item until the result is determined.

```yaml
Type: ScriptBlock
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ProgressAction

Sets the action preference for progress stream records generated by this cmdlet. See [about_Preference_Variables](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_preference_variables).

```yaml
Type: ActionPreference
Parameter Sets: (All)
Aliases: proga

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Management.Automation.PSObject[]

## OUTPUTS

### System.Boolean

## NOTES

- Short-circuits: processing stops as soon as the result is determined. This makes `Test-Collection` more efficient than filtering with `Where-Object` and checking the count when dealing with large collections.
- `-All` on an empty collection returns `$true` (vacuous truth). This is consistent with LINQ's `Enumerable.All` and standard logic.
- `-Any` on an empty collection returns `$false`.
- The predicate script block should return exactly one boolean value. Returning non-boolean values or multiple values produces a warning, and the result may be incorrect.

## RELATED LINKS

[Where-Object](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/where-object)
