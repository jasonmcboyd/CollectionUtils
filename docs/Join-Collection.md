---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# Join-Collection

## SYNOPSIS
Joins two `IEnumerable` together.

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
Joins two `IEnumerable` together.

## EXAMPLES

### Example 1
```powershell
Join-Collection -Left (0..1) -Right (4..5) -CrossJoin

Left Right
---- -----
   0     4
   0     5
   1     4
   1     5
```

Performs a cross join of the left and right collections.

### Example 2
```powershell
Join-Collection -Left (0..1) -Right (4..6) -ZipJoin

Left Right
---- -----
   0     4
   1     5
         6
```

Performs a zip join of the left and right collections. The right collection is longer than the left collection so the unmatched items in the right collection are paired with null.

## PARAMETERS

### -Comparer
The collection of `IEqualityComparer` to use when comparing each property of the key. If this is not specified the default comparer for each property type will be used unless that type is a string in which case the `DefaultStringComparer` will be used.

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
Performs a cross join. Every item in the left collection is paired with every item in the right collection.

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
The default string comparer to use when comparing string keys. The default value is `OrdinalIgnoreCase`. The `Comparer` property takes precedence over this property.

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
Performs a disjunct join. Only items that are in the left or right collection but not both will be returned.

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
Performs an inner join. Only items that are in both the left and right collection will be returned.

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
A collection of property names and/or script blocks to use as the key for both, the left and right collections.

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
The action to take when a key collision occurs. The default value is `Error`.

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
The left collection.

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
Performs a left join. Every item in the left collection is paired with items in the right collection with matching keys. If there is no match in the right collection the right item will be null.

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
A collection of property names and/or script blocks to use as the key for the left collection. The key names must match the key names of `RightKey`.

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
Performs an outer join. Every item in the left collection is paired with items in the right collection with matching keys. If there is no match in the right collection the right item will be null. If there is no match in the left collection the left item will be null.

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
The right collection.

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
Performs a right join. Every item in the right collection is paired with items in the left collection with matching keys. If there is no match in the left collection the left item will be null.

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
A collection of property names and/or script blocks to use as the key for the right collection. The key names must match the key names of `LeftKey`.

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
Performs a zip join, pairing items based on their position: the first item from the left collection is paired with the first from the right, the second with the second, and so on. If one collection is shorter, the unmatched items in the other collection will be paired with null.

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
