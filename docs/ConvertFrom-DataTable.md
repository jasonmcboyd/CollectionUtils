---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# ConvertFrom-DataTable

## SYNOPSIS
Converts a collection of DataTables or DataRows to a collection of PSObjects.

## SYNTAX

### Table
```
ConvertFrom-DataTable [-Table] <DataTable[]> [<CommonParameters>]
```

### Row
```
ConvertFrom-DataTable [-Row] <DataRow[]> [<CommonParameters>]
```

## DESCRIPTION
Converts `[System.Data.DataTable[]]` or `[System.Data.DataRow[]]` to a collection of PSObjects. Each column on in the DataTable or DataRow will be a property on the PSObject.

## EXAMPLES

### Example 1

```powershell
ConvertFrom-DataTable -Table $table
```

Converts DataTables to a collection of PSObjects.

### Example 2

```powershell
ConvertFrom-DataTable $table
```

Infers the input type, binds it to the `Table` parameter, and converts it to a collection of PSObjects.

### Example 3

```powershell
$table | ConvertFrom-DataTable
```

Pipes DataTables and converts them to a collection of PSObjects.

### Example 4

```powershell
ConvertFrom-DataTable -Row $table.Where({ $_['Age'] -eq 19 })
```

Converts DataRows to a collection of PSObjects.

### Example 5

```powershell
ConvertFrom-DataTable $table.Where({ $_['Age'] -eq 19 })
```

Infers the input type, binds it to the `Row` parameter, and converts it to a collection of PSObjects.

### Example 6

```powershell
$table.Where({ $_['Age'] -eq 19 }) | ConvertFrom-DataTable
```

Pipes DataRows and converts them to a collection of PSObjects.

## PARAMETERS

### -Row
{{ Fill Row Description }}

```yaml
Type: DataRow[]
Parameter Sets: Row
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Table
{{ Fill Table Description }}

```yaml
Type: DataTable[]
Parameter Sets: Table
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Data.DataTable[]

### System.Data.DataRow[]

## OUTPUTS

### System.Management.Automation.PSObject[]

## NOTES

## RELATED LINKS
