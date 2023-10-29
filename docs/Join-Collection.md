---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# Join-Collection

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

### CrossJoin
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-CrossJoin] [<CommonParameters>]
```

### ZipJoin
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-ZipJoin] [<CommonParameters>]
```

### DisjunctJoin|Key
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-DisjunctJoin] [-Key] <KeyParameter[]>
 [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [<CommonParameters>]
```

### DisjunctJoin|LeftKey|RightKey
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-DisjunctJoin] [-LeftKey] <KeyParameter[]>
 [-RightKey] <KeyParameter[]> [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [<CommonParameters>]
```

### InnerJoin|Key
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-InnerJoin] [-Key] <KeyParameter[]>
 [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [<CommonParameters>]
```

### InnerJoin|LeftKey|RightKey
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-InnerJoin] [-LeftKey] <KeyParameter[]>
 [-RightKey] <KeyParameter[]> [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [<CommonParameters>]
```

### LeftJoin|Key
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-LeftJoin] [-Key] <KeyParameter[]>
 [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [<CommonParameters>]
```

### LeftJoin|LeftKey|RightKey
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-LeftJoin] [-LeftKey] <KeyParameter[]>
 [-RightKey] <KeyParameter[]> [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [<CommonParameters>]
```

### OuterJoin|Key
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-OuterJoin] [-Key] <KeyParameter[]>
 [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [<CommonParameters>]
```

### OuterJoin|LeftKey|RightKey
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-OuterJoin] [-LeftKey] <KeyParameter[]>
 [-RightKey] <KeyParameter[]> [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [<CommonParameters>]
```

### RightJoin|Key
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-RightJoin] [-Key] <KeyParameter[]>
 [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [<CommonParameters>]
```

### RightJoin|LeftKey|RightKey
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-RightJoin] [-LeftKey] <KeyParameter[]>
 [-RightKey] <KeyParameter[]> [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [<CommonParameters>]
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
Parameter Sets: DisjunctJoin|Key, DisjunctJoin|LeftKey|RightKey, InnerJoin|Key, InnerJoin|LeftKey|RightKey, LeftJoin|Key, LeftJoin|LeftKey|RightKey, OuterJoin|Key, OuterJoin|LeftKey|RightKey, RightJoin|Key, RightJoin|LeftKey|RightKey
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CrossJoin
{{ Fill CrossJoin Description }}

```yaml
Type: SwitchParameter
Parameter Sets: CrossJoin
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DefaultStringComparer
{{ Fill DefaultStringComparer Description }}

```yaml
Type: System.Collections.Generic.IEqualityComparer`1[System.String]
Parameter Sets: DisjunctJoin|Key, DisjunctJoin|LeftKey|RightKey, InnerJoin|Key, InnerJoin|LeftKey|RightKey, LeftJoin|Key, LeftJoin|LeftKey|RightKey, OuterJoin|Key, OuterJoin|LeftKey|RightKey, RightJoin|Key, RightJoin|LeftKey|RightKey
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DisjunctJoin
{{ Fill DisjunctJoin Description }}

```yaml
Type: SwitchParameter
Parameter Sets: DisjunctJoin|Key, DisjunctJoin|LeftKey|RightKey
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InnerJoin
{{ Fill InnerJoin Description }}

```yaml
Type: SwitchParameter
Parameter Sets: InnerJoin|Key, InnerJoin|LeftKey|RightKey
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Key
{{ Fill Key Description }}

```yaml
Type: KeyParameter[]
Parameter Sets: DisjunctJoin|Key, InnerJoin|Key, LeftJoin|Key, OuterJoin|Key, RightJoin|Key
Aliases:

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -KeyCollisionPreference
{{ Fill KeyCollisionPreference Description }}

```yaml
Type: JoinCollectionKeyCollisionPreference
Parameter Sets: DisjunctJoin|Key, DisjunctJoin|LeftKey|RightKey, InnerJoin|Key, InnerJoin|LeftKey|RightKey, LeftJoin|Key, LeftJoin|LeftKey|RightKey, OuterJoin|Key, OuterJoin|LeftKey|RightKey, RightJoin|Key, RightJoin|LeftKey|RightKey
Aliases:
Accepted values: Error, Group, GroupThenFlatten, Ignore, Warn

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Left
{{ Fill Left Description }}

```yaml
Type: IEnumerable
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LeftJoin
{{ Fill LeftJoin Description }}

```yaml
Type: SwitchParameter
Parameter Sets: LeftJoin|Key, LeftJoin|LeftKey|RightKey
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LeftKey
{{ Fill LeftKey Description }}

```yaml
Type: KeyParameter[]
Parameter Sets: DisjunctJoin|LeftKey|RightKey, InnerJoin|LeftKey|RightKey, LeftJoin|LeftKey|RightKey, OuterJoin|LeftKey|RightKey, RightJoin|LeftKey|RightKey
Aliases:

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OuterJoin
{{ Fill OuterJoin Description }}

```yaml
Type: SwitchParameter
Parameter Sets: OuterJoin|Key, OuterJoin|LeftKey|RightKey
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Right
{{ Fill Right Description }}

```yaml
Type: IEnumerable
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RightJoin
{{ Fill RightJoin Description }}

```yaml
Type: SwitchParameter
Parameter Sets: RightJoin|Key, RightJoin|LeftKey|RightKey
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RightKey
{{ Fill RightKey Description }}

```yaml
Type: KeyParameter[]
Parameter Sets: DisjunctJoin|LeftKey|RightKey, InnerJoin|LeftKey|RightKey, LeftJoin|LeftKey|RightKey, OuterJoin|LeftKey|RightKey, RightJoin|LeftKey|RightKey
Aliases:

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ZipJoin
{{ Fill ZipJoin Description }}

```yaml
Type: SwitchParameter
Parameter Sets: ZipJoin
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Management.Automation.PSObject[]

## NOTES

## RELATED LINKS
