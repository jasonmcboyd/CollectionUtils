# CollectionUtils

## Installation

`Install-Module CollectionUtils -AllowPrerelease`

## Commands

- [`ConvertFrom-DataTable`](###ConvertFrom-DataTable): Converts a `System.Data.DataTable` to a collection of PSObjects.
- [`ConvertTo-Hashtable`](###ConvertTo-Hashtable): Converts a collection of objects to a `Hashtable`.
- [`Join-Collection`](###Join-Collection): Joins two collections together.
- [`Test-Collection`](###Test-Collection): Tests any or all items in a collections.

### ConvertFrom-DataTable

Assume we have a `System.Data.DataTable` variable `$table` that contains the following data:

| Id | Name    | Age |
|----|---------|-----|
| 1  | Alice   | 21  |
| 2  | Bob     | 20  |
| 3  | Charlie | 19  |
| 4  | David   | 21  |
| 5  | Eve     | 19  |

#### Example 1

```powershell
ConvertFrom-DataTable -Table $table
```

#### Example 2

```powershell
ConvertFrom-DataTable $table
```

#### Example 3

```powershell
$table | ConvertFrom-DataTable
```

#### Example 4

```powershell
ConvertFrom-DataTable -Row $table.Where({ $_['Age'] -eq 19 })
```

#### Example 5

```powershell
ConvertFrom-DataTable -Row $table.Where({ $_['Age'] -eq 19 })
```

#### Example 6

```powershell
ConvertFrom-DataTable $table.Where({ $_['Age'] -eq 19 })
```

### ConvertTo-Hashtable

#### Example 1

```powershell
$objs = @(@{ Id = 1; Name = 'Jason' }, @{ Id = 2; Name = 'Bob' })

ConvertTo-Hashtable -InputObject $objs -Key Id

Name                           Value
----                           -----
2                              {[Name, Bob], [Id, 2]}
1                              {[Name, Jason], [Id, 1]}
```

#### Example 2

String comparers are case insensitive by default.

```powershell
$objs = @(@{ Id = 1; Name = 'Jason' }, @{ Id = 2; Name = 'jason' })

ConvertTo-Hashtable -InputObject $objs -Key Name
```

> ConvertTo-Hashtable: Key collision detected for key jason.

#### Example 3


```powershell
$objs = @(@{ Id = 1; Name = 'Jason' }, @{ Id = 2; Name = 'jason' })

ConvertTo-Hashtable -InputObject $objs -Key Name -DefaultStringComparer ([StringComparer]::Ordinal)

Name                           Value
----                           -----
Jason                          {[Id, 1], [Name, Jason]}
jason                          {[Id, 2], [Name, jason]}
```

> ConvertTo-Hashtable: Key collision detected for key jason.

#### Example 4

```powershell
ConvertTo-Hashtable -InputObject $objs -Key Name -KeyCollisionPreference Warn
<font color="yellow">WARNING: Key collision detected for key jason.</font>

Name                           Value
----                           -----
Jason                          {[Id, 1], [Name, Jason]}
```


### Join-Collection

### Test-Collection
