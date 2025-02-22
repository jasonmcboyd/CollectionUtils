---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# Import-SmartExcel

## SYNOPSIS
{{ Fill in the Synopsis }}

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
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Path
{{ Fill Path Description }}

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
{{ Fill SheetIndex Description }}

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
{{ Fill SheetName Description }}

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
{{ Fill ProgressAction Description }}

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

## RELATED LINKS
