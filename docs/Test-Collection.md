---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# Test-Collection

## SYNOPSIS
Tests that any or all items in a collection meet a condition.

## SYNTAX

### InputObject|PredicateScript|All
```
Test-Collection [-PredicateScript] <ScriptBlock> [-InputObject] <PSObject[]> [-All] [<CommonParameters>]
```

### InputObject|PredicateScript|Any
```
Test-Collection [-PredicateScript] <ScriptBlock> [-InputObject] <PSObject[]> [-Any] [<CommonParameters>]
```

## DESCRIPTION
Tests that any or all items in a collection meet a condition.

## EXAMPLES

### Example 1
```powershell
$objs = 0..10

Test-Collection -PredicateScript { $_ % 2 -eq 0 } -InputObject $objs -Any

True
```

Tests that any elements in the collection are even.

### Example 2
```powershell
$objs = 0..10

Test-Collection { $_ % 2 -eq 0 } $objs -Any

True
```

Tests that any elements in the collection are even using positional parameters.

### Example 3
```powershell
0..10 | Test-Collection { $_ % 2 -eq 0 } -Any

True
```

Pipes the input collection and tests that any elements in the collection are even using positional parameters.

### Example 4
```powershell
0..10 | Test-Collection { $_ % 2 -eq 0 } -All

False
```

Tests that all elements in the collection are even.

## PARAMETERS

### -All
Switch to test if all items in the collection meet the condition.

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
Switch to test if any items in the collection meet the condition.

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
The collection of objects to test.

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
The script block to test each item in the collection. The script block must return a boolean value.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Management.Automation.PSObject[]

## OUTPUTS

### System.Boolean

## NOTES

## RELATED LINKS
