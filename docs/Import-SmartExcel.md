---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# Import-SmartExcel

## SYNOPSIS

Imports an Excel workbook and returns worksheet data as PSObjects with automatic type inference.

## SYNTAX

### Path (Default)
```
Import-SmartExcel [-Path] <String> [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

### SheetName
```
Import-SmartExcel [-Path] <String> [[-SheetName] <String[]>] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### SheetIndex
```
Import-SmartExcel [-Path] <String> [[-SheetIndex] <Int32[]>] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

## DESCRIPTION

Import-SmartExcel reads an Excel workbook (.xlsx or .xls) and returns one PSObject per worksheet. Each worksheet PSObject has two properties:

- **WorksheetName** (String): The name of the worksheet tab.

- **Data** (PSObject[]): The rows from the worksheet as PSObjects, with column names as property names and values converted to their inferred types.

By default, all worksheets in the workbook are imported. Use -SheetName to filter by worksheet name or -SheetIndex to filter by zero-based position. These two parameters are mutually exclusive.

Type inference is applied per column within each worksheet and uses the same logic as Import-SmartCsv and ConvertFrom-SmartCsv:

1. **Boolean** — All non-null values are "true" or "false" (case-insensitive), or all integer values are only "0" and "1".

2. **Int32** — All non-null values are whole numbers (at least one value outside the 0/1 range).

3. **Decimal** — All non-null values are numbers and at least one has a fractional part.

4. **DateTime** — All non-null values parse as dates using the invariant culture.

5. **String** — Any column that does not satisfy the above conditions.

The first row of each worksheet is treated as the header row. Null header cells receive fallback names ("Column0", "Column1", etc., using the column's zero-based index). Empty worksheets are handled gracefully and return an empty Data array.

Paths are resolved against PowerShell's current working directory ($PWD). The file is opened with shared read access, so Import-SmartExcel can read workbooks that are currently open in Excel.

## EXAMPLES

### Example 1: Import all worksheets from a workbook

```powershell
Import-SmartExcel -Path .\workbook.xlsx
```

Returns one PSObject per worksheet. Each object has a WorksheetName property and a Data property containing the typed rows from that sheet.


### Example 2: Import a single worksheet by name

```powershell
Import-SmartExcel -Path .\workbook.xlsx -SheetName 'Sales'
```

Imports only the worksheet named "Sales". The sheet name comparison is case-insensitive. The result is a single PSObject with WorksheetName set to "Sales" and Data containing the rows from that sheet.


### Example 3: Import multiple worksheets by index

```powershell
Import-SmartExcel -Path .\workbook.xlsx -SheetIndex 0, 2
```

Imports only the first and third worksheets (zero-based indices 0 and 2). Useful when you know the sheet positions but not their names.


### Example 4: Access and filter worksheet data

```powershell
$result = Import-SmartExcel .\quarterly_report.xlsx

foreach ($sheet in $result) {
    Write-Host "Sheet: $($sheet.WorksheetName) — $($sheet.Data.Count) rows"
}

# Filter rows from the first sheet
$result[0].Data | Where-Object { $_.Revenue -gt 10000 } | Format-Table
```

Iterates over all worksheets to display row counts, then filters rows in the first sheet by a numeric Revenue column. Because Revenue is inferred as a numeric type, the -gt comparison is numeric rather than lexicographic.

## PARAMETERS

### -Path

The path to the Excel file to import (.xlsx or .xls). Supports PowerShell-relative paths, which are resolved against $PWD. The file is opened with FileShare.ReadWrite, so it can be read even when another process such as Excel has the file open. Accepts pipeline input by value.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -SheetIndex

One or more zero-based worksheet indices to import. Only worksheets at the specified positions are returned. If not specified, all worksheets are imported. Mutually exclusive with -SheetName.

```yaml
Type: Int32[]
Parameter Sets: SheetIndex
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SheetName

One or more worksheet names to import. Only worksheets whose names match (case-insensitive) are returned. If not specified, all worksheets are imported. Mutually exclusive with -SheetIndex.

```yaml
Type: String[]
Parameter Sets: SheetName
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ProgressAction

Specifies how PowerShell responds to progress updates generated by a script, cmdlet, or provider. This is a standard PowerShell common parameter.

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

### System.String

## OUTPUTS

### System.Management.Automation.PSObject[]

## NOTES

- Each output PSObject represents one worksheet and has two properties: WorksheetName (String) and Data (PSObject[]).
- Null header cells receive fallback column names based on their zero-based column index (e.g., "Column0", "Column2").
- Empty worksheets return a PSObject with an empty Data array rather than being skipped.
- -SheetName matching is case-insensitive.
- -SheetIndex is zero-based. The first sheet in the workbook is index 0.
- Paths are resolved using PowerShell's session state, so PowerShell drive mappings and relative paths work correctly.
- Files are opened with FileShare.ReadWrite. Files locked exclusively will still produce an access error.
- Supports both .xlsx and .xls formats via ExcelDataReader.

## RELATED LINKS

[Import-SmartCsv](Import-SmartCsv.md)
[ConvertFrom-SmartCsv](ConvertFrom-SmartCsv.md)
