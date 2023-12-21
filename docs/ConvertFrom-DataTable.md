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
ConvertFrom-DataTable $table
$table | ConvertFrom-DataTable
```

Converts DataTables to a collection of PSObjects.

### Example 2

```powershell
ConvertFrom-DataTable -Row $table.Where({ $_['Age'] -eq 19 })
ConvertFrom-DataTable $table.Where({ $_['Age'] -eq 19 })
$table.Where({ $_['Age'] -eq 19 }) | ConvertFrom-DataTable
```

Converts DataRows to a collection of PSObjects.

## PARAMETERS

### -Row
A collection of `System.Data.DataTable` to convert to a collection of PSObjects.

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
A collection of `System.Data.DataRow` to convert to a collection of PSObjects.

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
