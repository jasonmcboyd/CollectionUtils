---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# ConvertTo-Hashtable

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

```
ConvertTo-Hashtable [-InputObject] <PSObject[]> [-Key] <KeyParameter[]> [[-Comparer] <KeyComparerParameter>]
 [[-DefaultStringComparer] <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <ConvertToHashtableKeyCollisionPreference>] [<CommonParameters>]
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

### -Comparer
{{ Fill Comparer Description }}

```yaml
Type: KeyComparerParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DefaultStringComparer
{{ Fill DefaultStringComparer Description }}

```yaml
Type: System.Collections.Generic.IEqualityComparer`1[System.String]
Parameter Sets: (All)
Aliases:

Required: False
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InputObject
{{ Fill InputObject Description }}

```yaml
Type: PSObject[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Key
{{ Fill Key Description }}

```yaml
Type: KeyParameter[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -KeyCollisionPreference
{{ Fill KeyCollisionPreference Description }}

```yaml
Type: ConvertToHashtableKeyCollisionPreference
Parameter Sets: (All)
Aliases:
Accepted values: Error, Group, Ignore, Warn

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

### System.Collections.Hashtable

## NOTES

## RELATED LINKS
