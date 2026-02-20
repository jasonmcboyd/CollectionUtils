---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# ConvertTo-Hashtable

## SYNOPSIS

Converts a collection of PSObjects into a Hashtable indexed by one or more key properties.

## SYNTAX

```
ConvertTo-Hashtable [-InputObject] <PSObject[]> [-Key] <KeyParameter[]> [[-Comparer] <KeyComparerParameter>]
 [[-DefaultStringComparer] <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <ConvertToHashtableKeyCollisionPreference>] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

## DESCRIPTION

`ConvertTo-Hashtable` builds a lookup table from a collection of objects. You specify one or more key properties, and each object becomes a value in the resulting Hashtable indexed by those keys.

By default, keys are wrapped in a hashtable — a single key `Id` with value `5` produces a key of `@{Id = 5}`. Use `-ExpandKey` with a single key field to use the value directly (e.g., `5`) instead.

When multiple key fields are specified, the key is always a composite hashtable — `-ExpandKey` has no effect.

String key comparisons are case-insensitive by default (`OrdinalIgnoreCase`). Use `-DefaultStringComparer` or `-Comparer` to override this per-key or globally.

Duplicate keys produce a terminating error by default. Use `-KeyCollisionPreference` to group, warn, or silently ignore duplicates instead.

## EXAMPLES

### Example 1: Convert a collection to a hashtable keyed by a single property

```powershell
$users = @(
    [PSCustomObject]@{ Id = 1; Name = 'Alice' }
    [PSCustomObject]@{ Id = 2; Name = 'Bob' }
    [PSCustomObject]@{ Id = 3; Name = 'Carol' }
)

$lookup = $users | ConvertTo-Hashtable -Key 'Id' -ExpandKey

$lookup[2].Name  # Returns 'Bob'
```

Using `-ExpandKey` with a single key field lets you index directly by the value (`2`) rather than by `@{Id = 2}`.


### Example 2: Key by a string property using pipeline input

```powershell
$lookup = Get-Process | ConvertTo-Hashtable -Key 'Name' -ExpandKey

$lookup['explorer'].Id
```

Pipes process objects into the cmdlet and builds a hashtable keyed by process name. Because process names are strings and the default comparer is `OrdinalIgnoreCase`, lookups are case-insensitive.


### Example 3: Use a composite key across multiple properties

```powershell
$people = @(
    [PSCustomObject]@{ FirstName = 'Jane'; LastName = 'Smith'; Dept = 'Engineering' }
    [PSCustomObject]@{ FirstName = 'Jane'; LastName = 'Doe';   Dept = 'Marketing' }
    [PSCustomObject]@{ FirstName = 'John'; LastName = 'Smith'; Dept = 'Sales' }
)

$lookup = $people | ConvertTo-Hashtable -Key 'FirstName', 'LastName'

$lookup[@{FirstName = 'Jane'; LastName = 'Smith'}].Dept  # Returns 'Engineering'
```

When multiple key fields are provided, the hashtable key is a composite hashtable containing all key field values.


### Example 4: Group objects that share the same key

```powershell
$orders = @(
    [PSCustomObject]@{ CustomerId = 10; OrderId = 'A' }
    [PSCustomObject]@{ CustomerId = 10; OrderId = 'B' }
    [PSCustomObject]@{ CustomerId = 20; OrderId = 'C' }
)

$grouped = $orders | ConvertTo-Hashtable -Key 'CustomerId' -ExpandKey -KeyCollisionPreference Group

$grouped[10].Count   # Returns 2
$grouped[10][0].OrderId  # Returns 'A'
```

`-KeyCollisionPreference Group` collects all objects sharing a key into an array rather than treating duplicate keys as an error.


### Example 5: Use a computed key with a script block expression

```powershell
$files = Get-ChildItem -File

$lookup = $files | ConvertTo-Hashtable -Key @{ NameLower = { $_.Name.ToLower() } } -ExpandKey

$lookup['readme.md']
```

Pass a hashtable as the key value to compute a derived key. The hashtable must have a single entry whose value is a script block; the entry's key name becomes the key field name.

## PARAMETERS

### -Comparer

A hashtable mapping key property names to custom `IEqualityComparer` instances. Use this to override the comparison behavior for specific key fields. The property name lookup in this hashtable is case-insensitive. Any property name specified here must match a name provided in `-Key`; unrecognized names produce a terminating error.

For example, `@{ Name = [StringComparer]::Ordinal }` makes the `Name` key field case-sensitive while other key fields retain the default comparer.

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

The `IEqualityComparer<string>` used for all string key comparisons that do not have a specific entry in `-Comparer`. Defaults to `[StringComparer]::OrdinalIgnoreCase`, which makes string key lookups case-insensitive. Set to `[StringComparer]::Ordinal` for case-sensitive matching.

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

### -ExpandKey

When specified with a single key field, the key value is used directly as the hashtable key rather than being wrapped in a hashtable. For example, with `-ExpandKey`, a key of `Id` with value `5` produces a hashtable key of `5` instead of `@{Id = 5}`. Has no effect when multiple key fields are specified.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -InputObject

The collection of PSObjects to convert. Accepts pipeline input. When passing an array directly (not via pipeline), wrap it in a subexpression (e.g., `ConvertTo-Hashtable -InputObject $items`) to avoid PowerShell unwrapping the array.

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

The property name(s) to use as hashtable keys. Accepts one or more of:

- A string naming a property on the input objects (e.g., `'Id'`)
- A hashtable with a single entry whose value is a script block, which computes a derived key (e.g., `@{ LowerId = { $_.Id.ToString().ToLower() } }`)

When a single key is specified and `-ExpandKey` is present, the key value is used as-is. When multiple keys are specified, the hashtable key is always a composite hashtable of key field name/value pairs.

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

Controls what happens when two input objects produce the same key. Accepted values:

- `Error` (default): Writes a terminating error on the first collision.
- `Warn`: Writes a warning and retains the first object; subsequent duplicates are discarded.
- `Ignore`: Silently retains the first object; subsequent duplicates are discarded.
- `Group`: Collects all objects with the same key into an array value.

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

### System.Collections.Hashtable

## NOTES

- String key comparisons are case-insensitive by default (`OrdinalIgnoreCase`). Use `-DefaultStringComparer [StringComparer]::Ordinal` for case-sensitive keys.
- Without `-ExpandKey`, a single key is still wrapped in a hashtable. Use `-ExpandKey` whenever you want to index by a plain value.
- Duplicate keys produce a terminating error by default. Choose a `-KeyCollisionPreference` value appropriate for your data.
- `-ExpandKey` has no effect when multiple key fields are specified; composite keys are always hashtables.

## RELATED LINKS

[Join-Collection](Join-Collection.md)
