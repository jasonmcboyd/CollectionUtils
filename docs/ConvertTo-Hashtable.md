---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# ConvertTo-Hashtable

## SYNOPSIS
Converts a collection of `PSObject` to a `Hashtable`.

## SYNTAX

```
ConvertTo-Hashtable [-InputObject] <PSObject[]> [-Key] <KeyParameter[]> [[-Comparer] <KeyComparerParameter>]
 [[-DefaultStringComparer] <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <ConvertToHashtableKeyCollisionPreference>] [<CommonParameters>]
```

## DESCRIPTION
Converts a `PSObject[]` to a `Hashtable` using the specified key. Users can specify which properties should be used as keys or even create custom keys with script blocks. Users also have control over how keys are compared and how the command should behave in the event of a key collision.

## EXAMPLES

### Example 1
```powershell
$objs = [pscustomobject]@{ Name = 'John'; Age = 19 }, [pscustomobject]@{ Name = 'Jane'; Age = 19 }

ConvertTo-Hashtable -InputObject $objs -Key Name

Name                           Value
----                           -----
Jane                           @{Name=Jane; Age=19}
John                           @{Name=John; Age=19}
```

Converts a collection of objects to a hashtable using the `Name` property as the key.

### Example 2
```powershell
$objs = [pscustomobject]@{ Name = 'John'; Age = 19 }, [pscustomobject]@{ Name = 'Jane'; Age = 19 }

$objs | ConvertTo-Hashtable -Key Name

Name                           Value
----                           -----
Jane                           @{Name=Jane; Age=19}
John                           @{Name=John; Age=19}
```

Pipes a collection of objects to `ConvertTo-Hashtable` using the `Name` property as the key.

### Example 3

Writes to the error stream:

> ConvertTo-Hashtable: Key collision detected for key 19

Then outputs a hashtable with the objects that did not collide on the key.

```powershell
$objs = [pscustomobject]@{ Name = 'John'; Age = 19 }, [pscustomobject]@{ Name = 'Jane'; Age = 19 }

$objs | ConvertTo-Hashtable -Key Age
```

```
Name                           Value
----                           -----
John                           @{Name=John; Age=19}
```

### Example 4
Writes to the warning stream::

> WARNING: Key collision detected for key 19.

Then outputs a hashtable with the objects that did not collide on the key.

```powershell
$objs = [pscustomobject]@{ Name = 'John'; Age = 19 }, [pscustomobject]@{ Name = 'Jane'; Age = 19 }

$objs | ConvertTo-Hashtable -Key Age -KeyCollisionPreference Warn
```

```
Name                           Value
----                           -----
John                           @{Name=John; Age=19}
```

### Example 5
```powershell
$objs = [pscustomobject]@{ Name = 'John'; Age = 19 }, [pscustomobject]@{ Name = 'Jane'; Age = 19 }

$objs | ConvertTo-Hashtable -Key Age -KeyCollisionPreference Ignore

Name                           Value
----                           -----
John                           @{Name=John; Age=19}
```

Outputs a hashtable with objects that did not collide on the key and ignores the objects that did collide on the key.

### Example 6
```powershell
$objs = [pscustomobject]@{ Name = 'John'; Age = 19 }, [pscustomobject]@{ Name = 'Jane'; Age = 19 }

$objs | ConvertTo-Hashtable -Key Age -KeyCollisionPreference Group

Name                           Value
----                           -----
John                           {@{Name=John; Age=19}, @{Name=Jane; Age=19}}
```

Outputs a hashtable with the objects that collided on the key grouped together.

### Example 7
```powershell
$objs = [pscustomobject]@{ Name = 'John'; Age = 19 }, [pscustomobject]@{ Name = 'john'; Age = 19 }

$objs | ConvertTo-Hashtable -Key Name -KeyCollisionPreference Group

Name                           Value
----                           -----
John                           {@{Name=John; Age=19}, @{Name=john; Age=19}}
```

Outputs a hashtable with the objects grouped because the `Name`, despite having different casing, are equal due to the default value of `DefaultStringComparer` being `OrdinalIgnoreCase`.

### Example 8
```powershell
$objs = [pscustomobject]@{ Name = 'John'; Age = 19 }, [pscustomobject]@{ Name = 'john'; Age = 19 }

$objs | ConvertTo-Hashtable -Key Name -DefaultStringComparer ([System.StringComparer]::Ordinal)

Name                           Value
----                           -----
John                           {@{Name=John; Age=19}, @{Name=john; Age=19}}
```

Outputs a hashtable with the objects not grouped because the `Name` are not equal due to the `DefaultStringComparer` being `Ordinal`.

### Example 9
```powershell
$objs = @(
  [pscustomobject]@{ Name = 'John'; Age = 19 }
  [pscustomobject]@{ Name = 'John'; Age = 19 }
  [pscustomobject]@{ Name = 'Jane'; Age = 19 }
  [pscustomobject]@{ Name = 'John'; Age = 20 } )

$hashtable = $objs | ConvertTo-Hashtable -Key Name, Age -KeyCollisionPreference Group

$hashtable

Name                           Value
----                           -----
{[Age, 19], [Name, Jane]}      {@{Name=Jane; Age=19}}
{[Age, 20], [Name, John]}      {@{Name=John; Age=20}}
{[Age, 19], [Name, John]}      {@{Name=John; Age=19}, @{Name=John; Age=19}}

$hashtable[@{ Age = 19; Name = 'Jane' }]

Name Age
---- ---
Jane  19
```

In this example, a hashtable is created with multi-property keys. These keys are themselves hashtables. Although in .NET, reference types are typically compared by reference, this command creates a custom comparer internally to assess key equality based on their properties. This approach ensures values can be effectively retrieved using hashtable keys.

### Example 10
```powershell
$objs = @(
  [pscustomobject]@{ FirstName = 'John'; LastName = 'Smith'; Age = 19 }
  [pscustomobject]@{ FirstName = 'john'; LastName = 'Smith'; Age = 20 }
  [pscustomobject]@{ FirstName = 'john'; LastName = 'smith'; Age = 21 } )

$objs | ConvertTo-Hashtable -Key FirstName, LastName -Comparer @{ LastName = [StringComparer]::Ordinal } -KeyCollisionPreference Group

Name                                   Value
----                                   -----
{[FirstName, john], [LastName, smith]} {@{FirstName=john; LastName=smith; Age=21}}
{[FirstName, John], [LastName, Smith]} {@{FirstName=John; LastName=Smith; Age=19}, @{FirstName=john; LastName=Smith; Age=20}}
```

Passes multiple string properties to the `Key` parameter. The comparer for the `LastName` property is overridden to be case sensitive. This results in using a case insensitive comparison for the `FirstName` property and a case sensitive comparison for the `LastName` property.

### Example 11
```powershell
$objs = [pscustomobject]@{ Name = 'John'; Age = 19 }, [pscustomobject]@{ Name = 'Jane'; Age = 17 }

$objs | ConvertTo-Hashtable -Key @{ IsAdult = { $_.Age -ge 18 } }

Name                           Value
----                           -----
True                           @{Name=John; Age=19}
False                          @{Name=Jane; Age=17}
```

Passes a script block to the `Key` parameter, creating a custom key field, and returns a hashtable with the result of the script block as the key.

### Example 12
```powershell
$index = -1

0..14 | { $index++; $_ } | ConvertTo-HashTable -Key @{ Bucket = { [int][Math]::Floor($index / 5) } } -KeyCollisionPreference Group
```

Partitions a collection into buckets of 5 items each.

## PARAMETERS

### -Comparer
The collection of `IEqualityComparer` to use when comparing each property of the key. If this is not specified the default comparer for each property type will be used, unless that type is a string, in which case the `DefaultStringComparer` will be used.

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
The default string comparer to use when comparing string keys. The default value is `OrdinalIgnoreCase`. The `Comparer` property takes precedence over this property.

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
The collection of objects to convert to a hashtable.

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
A collection of property names and/or script blocks to use as the key for the hashtable.

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
The action to take when a key collision occurs. The default value is `Error`.

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
