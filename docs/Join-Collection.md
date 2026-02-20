---
external help file: CollectionUtils.dll-Help.xml
Module Name: CollectionUtils
online version:
schema: 2.0.0
---

# Join-Collection

## SYNOPSIS

Performs SQL-style joins between two PowerShell collections.

## SYNTAX

### CrossJoin
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-CrossJoin] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### ZipJoin
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-ZipJoin] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### DisjunctJoin|Key
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-DisjunctJoin] [-Key] <KeyParameter[]>
 [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [-ExpandKey] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### DisjunctJoin|LeftKey|RightKey
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-DisjunctJoin] [-LeftKey] <KeyParameter[]>
 [-RightKey] <KeyParameter[]> [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [-ExpandKey] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### InnerJoin|Key
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-InnerJoin] [-Key] <KeyParameter[]>
 [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [-ExpandKey] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### InnerJoin|LeftKey|RightKey
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-InnerJoin] [-LeftKey] <KeyParameter[]>
 [-RightKey] <KeyParameter[]> [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [-ExpandKey] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### LeftJoin|Key
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-LeftJoin] [-Key] <KeyParameter[]>
 [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [-ExpandKey] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### LeftJoin|LeftKey|RightKey
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-LeftJoin] [-LeftKey] <KeyParameter[]>
 [-RightKey] <KeyParameter[]> [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [-ExpandKey] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### OuterJoin|Key
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-OuterJoin] [-Key] <KeyParameter[]>
 [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [-ExpandKey] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### OuterJoin|LeftKey|RightKey
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-OuterJoin] [-LeftKey] <KeyParameter[]>
 [-RightKey] <KeyParameter[]> [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [-ExpandKey] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### RightJoin|Key
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-RightJoin] [-Key] <KeyParameter[]>
 [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [-ExpandKey] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

### RightJoin|LeftKey|RightKey
```
Join-Collection [-Left] <IEnumerable> [-Right] <IEnumerable> [-RightJoin] [-LeftKey] <KeyParameter[]>
 [-RightKey] <KeyParameter[]> [-Comparer <KeyComparerParameter>]
 [-DefaultStringComparer <System.Collections.Generic.IEqualityComparer`1[System.String]>]
 [-KeyCollisionPreference <JoinCollectionKeyCollisionPreference>] [-ExpandKey] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

## DESCRIPTION

`Join-Collection` merges two collections using SQL-style join semantics. It supports seven join types that cover the full range of relational join operations:

- **InnerJoin** — Returns only items whose keys match in both collections. Use this when you only care about records that exist on both sides.

- **LeftJoin** — Returns all items from the left collection. Where a matching right item exists the properties are merged; where no match exists the right-side properties are null. Use this when the left collection is your primary dataset and the right collection provides supplemental data.

- **RightJoin** — The mirror of LeftJoin. All right items are returned; left-side properties are null for unmatched right items.

- **OuterJoin** — Returns everything from both collections. Matched items are merged; unmatched items from either side appear with null properties for the missing side. Use this to see the complete picture across two datasets.

- **DisjunctJoin** — Returns only items that do not have a match in the other collection. This is the complement of InnerJoin and is useful for finding orphaned or mismatched records across two datasets.

- **CrossJoin** — Produces the Cartesian product: every item in Left is paired with every item in Right. No key is required. The output count equals Left.Count multiplied by Right.Count.

- **ZipJoin** — Pairs items by position: first with first, second with second, and so on. Stops when the shorter collection is exhausted. No key is required.

For the five key-based join types (Inner, Left, Right, Outer, Disjunct), keys can be a simple property name shared by both collections (`-Key 'Id'`) or separate property names for each side (`-LeftKey 'EmployeeId' -RightKey 'EmpId'`). Keys can also be computed on the fly using a hashtable with a script block expression, for example `@{ FullName = { "$($_.FirstName) $($_.LastName)" } }`.

String key comparisons are case-insensitive by default (`OrdinalIgnoreCase`). Use `-DefaultStringComparer` to change this globally, or `-Comparer` to override the comparer for specific key fields while leaving others at the default.

Neither `-Left` nor `-Right` accepts pipeline input. Both collections must be passed as parameter arguments.

## EXAMPLES

### Example 1: Inner join — match employees to their departments

```powershell
$employees = @(
    [PSCustomObject]@{ Name = 'Alice';   DepartmentId = 1 }
    [PSCustomObject]@{ Name = 'Bob';     DepartmentId = 2 }
    [PSCustomObject]@{ Name = 'Charlie'; DepartmentId = 99 }  # no matching department
)

$departments = @(
    [PSCustomObject]@{ DepartmentId = 1; DepartmentName = 'Engineering' }
    [PSCustomObject]@{ DepartmentId = 2; DepartmentName = 'Marketing' }
    [PSCustomObject]@{ DepartmentId = 3; DepartmentName = 'Finance' }    # no matching employee
)

Join-Collection -Left $employees -Right $departments -InnerJoin -Key 'DepartmentId'
```

```
Left                          Right                                         Key
----                          -----                                         ---
@{Name=Bob; DepartmentId=2}   @{DepartmentId=2; DepartmentName=Marketing}   {[DepartmentId, 2]}
@{Name=Alice; DepartmentId=1} @{DepartmentId=1; DepartmentName=Engineering} {[DepartmentId, 1]}
```

Each output object has `Left`, `Right`, and `Key` properties. `Left` and `Right` hold the original objects from each collection; `Key` holds the key value used for matching (wrapped in a hashtable by default). An InnerJoin returns only the rows where a key exists in both collections. Charlie (DepartmentId 99) has no matching department and is excluded. The Finance department (DepartmentId 3) has no matching employee and is excluded. This mirrors `SELECT * FROM employees INNER JOIN departments ON employees.DepartmentId = departments.DepartmentId` in SQL.


### Example 2: Left join — all employees, with department info where available

```powershell
Join-Collection -Left $employees -Right $departments -LeftJoin -Key 'DepartmentId'
```

```
Left                             Right                                         Key
----                             -----                                         ---
@{Name=Bob; DepartmentId=2}      @{DepartmentId=2; DepartmentName=Marketing}   {[DepartmentId, 2]}
@{Name=Charlie; DepartmentId=99}                                               {[DepartmentId, 99]}
@{Name=Alice; DepartmentId=1}    @{DepartmentId=1; DepartmentName=Engineering} {[DepartmentId, 1]}
```

A LeftJoin returns every item from the left collection regardless of whether a match exists on the right. Charlie appears with a null `Right` because DepartmentId 99 does not exist in the departments collection. The Finance department is still excluded because it has no match on the left. Use LeftJoin when the left collection is your authoritative list and the right collection provides optional supplemental data.


### Example 3: Right join — all departments, with employee info where available

```powershell
Join-Collection -Left $employees -Right $departments -RightJoin -Key 'DepartmentId'
```

```
Left                          Right                                         Key
----                          -----                                         ---
@{Name=Bob; DepartmentId=2}   @{DepartmentId=2; DepartmentName=Marketing}   {[DepartmentId, 2]}
@{Name=Alice; DepartmentId=1} @{DepartmentId=1; DepartmentName=Engineering} {[DepartmentId, 1]}
                              @{DepartmentId=3; DepartmentName=Finance}     {[DepartmentId, 3]}
```

A RightJoin is the mirror of LeftJoin. Every item from the right collection is returned. Finance appears with a null `Left` because no employee belongs to DepartmentId 3. Charlie is excluded because DepartmentId 99 has no match on the right. Use RightJoin when the right collection is your authoritative list.


### Example 4: Outer join — the complete picture across both collections

```powershell
Join-Collection -Left $employees -Right $departments -OuterJoin -Key 'DepartmentId'
```

```
Left                             Right                                         Key
----                             -----                                         ---
@{Name=Bob; DepartmentId=2}      @{DepartmentId=2; DepartmentName=Marketing}   {[DepartmentId, 2]}
@{Name=Charlie; DepartmentId=99}                                               {[DepartmentId, 99]}
@{Name=Alice; DepartmentId=1}    @{DepartmentId=1; DepartmentName=Engineering} {[DepartmentId, 1]}
                                 @{DepartmentId=3; DepartmentName=Finance}     {[DepartmentId, 3]}
```

An OuterJoin returns everything from both collections. Matched records have both `Left` and `Right` populated. Charlie appears with a null `Right` (his DepartmentId has no match on the right), and Finance appears with a null `Left` (its DepartmentId has no match on the left). This is the union of LeftJoin and RightJoin and corresponds to `FULL OUTER JOIN` in SQL.


### Example 5: Disjunct join — find the mismatches for data quality auditing

```powershell
Join-Collection -Left $employees -Right $departments -DisjunctJoin -Key 'DepartmentId'
```

```
Left                             Right                                     Key
----                             -----                                     ---
@{Name=Charlie; DepartmentId=99}                                           {[DepartmentId, 99]}
                                 @{DepartmentId=3; DepartmentName=Finance} {[DepartmentId, 3]}
```

A DisjunctJoin is the complement of InnerJoin — it returns only the records that did not find a match. Charlie has no department (null `Right`), and Finance has no employees (null `Left`). This is invaluable for data quality checks: finding orphaned foreign keys, records that failed to migrate, or items that exist in one system but not another. It corresponds to `FULL OUTER JOIN WHERE left.key IS NULL OR right.key IS NULL` in SQL.


### Example 6: Cross join — generate all size/color combinations for a product catalog

```powershell
$sizes = @(
    [PSCustomObject]@{ Size = 'Small' }
    [PSCustomObject]@{ Size = 'Medium' }
    [PSCustomObject]@{ Size = 'Large' }
)

$colors = @(
    [PSCustomObject]@{ Color = 'Red' }
    [PSCustomObject]@{ Color = 'Blue' }
    [PSCustomObject]@{ Color = 'Green' }
)

Join-Collection -Left $sizes -Right $colors -CrossJoin
```

```
Left           Right
----           -----
@{Size=Small}  @{Color=Red}
@{Size=Small}  @{Color=Blue}
@{Size=Small}  @{Color=Green}
@{Size=Medium} @{Color=Red}
@{Size=Medium} @{Color=Blue}
@{Size=Medium} @{Color=Green}
@{Size=Large}  @{Color=Red}
@{Size=Large}  @{Color=Blue}
@{Size=Large}  @{Color=Green}
```

Each output object has `Left` and `Right` properties holding the original objects from each side. A CrossJoin produces the Cartesian product of the two collections. Every item in Left is paired with every item in Right, producing 3 x 3 = 9 output rows. No key is needed. Use CrossJoin to generate combination matrices, test data sets, or scheduling grids.


### Example 7: Zip join — pair items positionally

```powershell
$questions = @(
    [PSCustomObject]@{ Number = 1; Question = 'What is the capital of France?' }
    [PSCustomObject]@{ Number = 2; Question = 'What is 7 times 8?' }
    [PSCustomObject]@{ Number = 3; Question = 'Who wrote Hamlet?' }
)

$answers = @(
    [PSCustomObject]@{ Answer = 'Paris' }
    [PSCustomObject]@{ Answer = '56' }
    [PSCustomObject]@{ Answer = 'Shakespeare' }
)

Join-Collection -Left $questions -Right $answers -ZipJoin
```

```
Left                                                 Right
----                                                 -----
@{Number=1; Question=What is the capital of France?} @{Answer=Paris}
@{Number=2; Question=What is 7 times 8?}             @{Answer=56}
@{Number=3; Question=Who wrote Hamlet?}               @{Answer=Shakespeare}
```

Like CrossJoin, ZipJoin output has `Left` and `Right` properties but no `Key`. A ZipJoin pairs items by position rather than by key value. The first left item is paired with the first right item, the second with the second, and so on. If the collections have different lengths, output stops when the shorter collection is exhausted. Use ZipJoin to merge two parallel arrays where positional order is the relationship.


### Example 8: Different key names — use LeftKey and RightKey

```powershell
$employees = @(
    [PSCustomObject]@{ Name = 'Alice'; EmployeeId = 'E001' }
    [PSCustomObject]@{ Name = 'Bob';   EmployeeId = 'E002' }
    [PSCustomObject]@{ Name = 'Carol'; EmployeeId = 'E003' }
)

$salaries = @(
    [PSCustomObject]@{ EmpId = 'E001'; AnnualSalary = 95000 }
    [PSCustomObject]@{ EmpId = 'E002'; AnnualSalary = 82000 }
    [PSCustomObject]@{ EmpId = 'E003'; AnnualSalary = 110000 }
)

Join-Collection -Left $employees -Right $salaries -InnerJoin -LeftKey 'EmployeeId' -RightKey 'EmpId' -ExpandKey
```

```
Left                           Right                              Key
----                           -----                              ---
@{Name=Alice; EmployeeId=E001} @{EmpId=E001; AnnualSalary=95000}  E001
@{Name=Bob; EmployeeId=E002}   @{EmpId=E002; AnnualSalary=82000}  E002
@{Name=Carol; EmployeeId=E003} @{EmpId=E003; AnnualSalary=110000} E003
```

When the two collections use different property names for the same logical key, use `-LeftKey` and `-RightKey` instead of `-Key`. Both parameters must have the same number of elements. `-ExpandKey` is required here because without it, the left key would be wrapped as `@{EmployeeId = 'E001'}` and the right key as `@{EmpId = 'E001'}` — these are different hashtables and would never match. With `-ExpandKey`, both sides compare the raw value (`'E001'`) directly, so the join succeeds.


### Example 9: Script block keys — compute join keys on the fly

```powershell
$nameIndex = @(
    [PSCustomObject]@{ FirstName = 'Alice'; LastName = 'Smith';  Score = 95 }
    [PSCustomObject]@{ FirstName = 'Bob';   LastName = 'Jones';  Score = 88 }
)

$badgeData = @(
    [PSCustomObject]@{ FullName = 'Alice Smith';  BadgeNumber = 'B-4421' }
    [PSCustomObject]@{ FullName = 'Bob Jones';    BadgeNumber = 'B-1192' }
)

$leftKey  = @{ FullName = { "$($_.FirstName) $($_.LastName)" } }
$rightKey = @{ FullName = { $_.FullName } }

Join-Collection -Left $nameIndex -Right $badgeData -InnerJoin -LeftKey $leftKey -RightKey $rightKey
```

```
Left                                         Right                                       Key
----                                         -----                                       ---
@{FirstName=Alice; LastName=Smith; Score=95} @{FullName=Alice Smith; BadgeNumber=B-4421} {[FullName, Alice Smith]}
@{FirstName=Bob; LastName=Jones; Score=88}   @{FullName=Bob Jones; BadgeNumber=B-1192}   {[FullName, Bob Jones]}
```

A key parameter can be a hashtable whose value is a script block. The hashtable key (`FullName` here) becomes the logical name of the computed field, and the script block is evaluated against each item to produce the match value. This lets you join on a derived value without modifying the source objects. Here, the left side concatenates `FirstName` and `LastName` to compute `FullName`, while the right side simply extracts the existing `FullName` property. Because both sides use the same key name (`FullName`), the key hashtables match without needing `-ExpandKey`.


### Example 10: ExpandKey — mixed key formats with scriptblock and simple string

```powershell
$nameIndex = @(
    [PSCustomObject]@{ FirstName = 'Alice'; LastName = 'Smith';  Score = 95 }
    [PSCustomObject]@{ FirstName = 'Bob';   LastName = 'Jones';  Score = 88 }
)

$badgeData = @(
    [PSCustomObject]@{ FullName = 'Alice Smith';  BadgeNumber = 'B-4421' }
    [PSCustomObject]@{ FullName = 'Bob Jones';    BadgeNumber = 'B-1192' }
)

$leftKey  = @{ FullName = { "$($_.FirstName) $($_.LastName)" } }
$rightKey = 'FullName'

Join-Collection -Left $nameIndex -Right $badgeData -InnerJoin -LeftKey $leftKey -RightKey $rightKey -ExpandKey
```

```
Left                                         Right                                       Key
----                                         -----                                       ---
@{FirstName=Bob; LastName=Jones; Score=88}   @{FullName=Bob Jones; BadgeNumber=B-1192}   Bob Jones
@{FirstName=Alice; LastName=Smith; Score=95} @{FullName=Alice Smith; BadgeNumber=B-4421} Alice Smith
```

Without `-ExpandKey`, the left key (a computed scriptblock) would be wrapped in a hashtable `@{FullName = 'Alice Smith'}`, while the right key (a plain string property name) would produce the raw string `'Alice Smith'`. These two forms don't match, producing zero results. `-ExpandKey` causes both sides to compare the raw values directly (`'Alice Smith'` vs `'Alice Smith'`), enabling a computed left key to match a simple right key seamlessly. Notice the `Key` column shows the unwrapped string value instead of a hashtable.


### Example 11: KeyCollisionPreference Group — one employee, multiple projects

```powershell
$employees = @(
    [PSCustomObject]@{ Name = 'Alice'; EmployeeId = 'E001' }
    [PSCustomObject]@{ Name = 'Bob';   EmployeeId = 'E002' }
)

$assignments = @(
    [PSCustomObject]@{ EmpId = 'E001'; Project = 'Alpha' }
    [PSCustomObject]@{ EmpId = 'E001'; Project = 'Beta' }
    [PSCustomObject]@{ EmpId = 'E001'; Project = 'Gamma' }
    [PSCustomObject]@{ EmpId = 'E002'; Project = 'Alpha' }
)

Join-Collection -Left $employees -Right $assignments -InnerJoin `
    -LeftKey 'EmployeeId' -RightKey 'EmpId' `
    -KeyCollisionPreference Group -ExpandKey |
    Format-List
```

```
Left  : {@{Name=Bob; EmployeeId=E002}}
Right : {@{EmpId=E002; Project=Alpha}}
Key   : E002

Left  : {@{Name=Alice; EmployeeId=E001}}
Right : {@{EmpId=E001; Project=Alpha}, @{EmpId=E001; Project=Beta}, @{EmpId=E001; Project=Gamma}}
Key   : E001
```

By default, duplicate keys in the right collection cause an error. Setting `-KeyCollisionPreference Group` instead collects all matching right items into an array in the `Right` property. Alice's `Right` value contains all three of her project assignments, while Bob has a single-element array. `-ExpandKey` is needed here because the left and right key names differ (`EmployeeId` vs `EmpId`). The output is piped to `Format-List` because the grouped arrays are easier to read in list format. Use `GroupThenFlatten` instead if you want a separate output object for each left/right pair rather than an array.


### Example 12: Custom comparer — case-sensitive matching on one field

```powershell
$products = @(
    [PSCustomObject]@{ SKU = 'widget-A'; Description = 'Widget Type A' }
    [PSCustomObject]@{ SKU = 'widget-a'; Description = 'Widget Type A (variant)' }
    [PSCustomObject]@{ SKU = 'widget-B'; Description = 'Widget Type B' }
)

$inventory = @(
    [PSCustomObject]@{ SKU = 'widget-A'; StockCount = 42 }
    [PSCustomObject]@{ SKU = 'widget-B'; StockCount = 17 }
)

$caseSensitive = @{ SKU = [StringComparer]::Ordinal }

Join-Collection -Left $products -Right $inventory -LeftJoin -Key 'SKU' -Comparer $caseSensitive
```

```
Left                                                 Right                          Key
----                                                 -----                          ---
@{SKU=widget-A; Description=Widget Type A}           @{SKU=widget-A; StockCount=42} {[SKU, widget-A]}
@{SKU=widget-a; Description=Widget Type A (variant)}                                {[SKU, widget-a]}
@{SKU=widget-B; Description=Widget Type B}           @{SKU=widget-B; StockCount=17} {[SKU, widget-B]}
```

By default all string key comparisons use `OrdinalIgnoreCase`, so `widget-A` and `widget-a` would be treated as the same key. The `-Comparer` parameter accepts a hashtable mapping field names to `IEqualityComparer` instances, overriding the default for that specific field. Here `[StringComparer]::Ordinal` makes the `SKU` comparison case-sensitive, so `widget-a` correctly finds no match and appears with a null `Right`. The `-DefaultStringComparer` parameter changes the default for all string key fields at once when you want global case-sensitive behavior.

## PARAMETERS

### -Comparer

A hashtable that maps key property names to custom `IEqualityComparer` instances. Use this to override the string comparison behavior for specific key fields. Property names in the hashtable are matched case-insensitively against the key field names.

For example, `@{ Name = [StringComparer]::Ordinal }` makes the `Name` key field use exact case-sensitive matching while any other key fields continue to use the default comparer. If a property name in the hashtable does not match any key field, a non-terminating error is written.

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

Performs a Cartesian product join. Every item in `-Left` is paired with every item in `-Right`, producing Left.Count multiplied by Right.Count output objects. No key parameters are used or required with this switch.

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

The default `IEqualityComparer<string>` used for all string key comparisons. Defaults to `[StringComparer]::OrdinalIgnoreCase`, which makes key matching case-insensitive. Set this to `[StringComparer]::Ordinal` to make all string key comparisons case-sensitive. To override the comparer for only specific fields, use `-Comparer` instead.

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

Returns only items that have no matching key in the other collection. This is the complement of `-InnerJoin`: items that would appear in an InnerJoin are excluded, and items that would not appear (the unmatched rows from both sides) are returned. Useful for finding orphaned records, failed migrations, or data quality issues across two datasets.

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

### -ExpandKey

When specified with a single key field, each item's key value is used directly for comparison rather than being wrapped in a hashtable. This allows a simple property name key on one side (e.g., `-RightKey 'FullName'`) to be matched against a computed scriptblock key on the other side (e.g., `-LeftKey @{ FullName = { ... } }`). Has no effect when multiple key fields are specified.

```yaml
Type: SwitchParameter
Parameter Sets: DisjunctJoin|Key, DisjunctJoin|LeftKey|RightKey, InnerJoin|Key, InnerJoin|LeftKey|RightKey, LeftJoin|Key, LeftJoin|LeftKey|RightKey, OuterJoin|Key, OuterJoin|LeftKey|RightKey, RightJoin|Key, RightJoin|LeftKey|RightKey
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -InnerJoin

Returns only items whose key exists in both the left and right collections. Items without a match on either side are excluded. Equivalent to `INNER JOIN` in SQL.

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

The property name(s) used to match items between the left and right collections. Use `-Key` when both collections share the same property name for the join field. Accepts one of the following for each element:

- A string: `'Id'` — matches on the property named `Id` in both collections.
- A hashtable with a script block value: `@{ ComputedName = { $_.Prop1 + $_.Prop2 } }` — evaluates the script block against each item and uses the result as the match value.

Multiple key fields can be specified as an array (for example, `-Key 'LastName', 'FirstName'`). When the left and right collections use different property names for the join field, use `-LeftKey` and `-RightKey` instead.

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

Controls the behavior when the right collection contains duplicate key values. The default is `Error`.

- `Error` — Writes a non-terminating error for each duplicate key and stops processing.
- `Warn` — Writes a warning for each duplicate key and uses the first matching right item.
- `Ignore` — Silently uses the first matching right item when duplicates are found.
- `Group` — Groups all right items that share the same key into an array. Each left item is joined to the array of matching right items, producing one output object per left item.
- `GroupThenFlatten` — Groups matching right items as with `Group`, then flattens the result so each left item is paired with each of its matching right items individually, producing one output object per left/right pair.

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

The left (first) input collection. Accepts any object that implements `IEnumerable`, including arrays, `ArrayList`, `PSObject` collections, and integer ranges such as `0..10`. This parameter does not accept pipeline input; pass the collection as an argument.

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

Returns all items from the left collection. Items with a matching key in the right collection have their properties merged. Items with no match appear in the output with null values for right-side properties. Equivalent to `LEFT OUTER JOIN` in SQL.

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

The key property name(s) to use from the left collection when the left and right collections use different property names for the join field. Must be paired with `-RightKey`, and both parameters must have the same number of elements. Accepts the same string or hashtable-with-script-block formats as `-Key`.

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

Returns all items from both collections. Items with matching keys are merged. Items from the left collection with no right match appear with null right-side properties, and items from the right collection with no left match appear with null left-side properties. Equivalent to `FULL OUTER JOIN` in SQL.

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

The right (second) input collection. Accepts the same types as `-Left`. For keyed joins, the right collection is fully materialized into memory before processing begins. This parameter does not accept pipeline input; pass the collection as an argument.

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

Returns all items from the right collection. Items with a matching key in the left collection have their properties merged. Items with no match appear in the output with null values for left-side properties. Equivalent to `RIGHT OUTER JOIN` in SQL.

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

The key property name(s) to use from the right collection when the left and right collections use different property names for the join field. Must be paired with `-LeftKey`, and both parameters must have the same number of elements. Accepts the same string or hashtable-with-script-block formats as `-Key`.

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

Pairs items by position. The first item in `-Left` is merged with the first item in `-Right`, the second with the second, and so on. Processing stops when the shorter collection is exhausted. No key parameters are used or required with this switch.

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

### -ProgressAction

Specifies how PowerShell responds to progress updates generated by this cmdlet. Accepts standard `ActionPreference` values (`SilentlyContinue`, `Continue`, etc.).

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

### None

Neither `-Left` nor `-Right` accepts pipeline input. Both collections must be passed as parameter arguments.

## OUTPUTS

### System.Management.Automation.PSObject[]

Each output object has `Left`, `Right`, and (for keyed joins) `Key` properties. `Left` and `Right` hold the original objects from each collection. `Key` holds the key value used for matching (a hashtable by default, or the raw value when `-ExpandKey` is used). For unmatched items (in LeftJoin, RightJoin, OuterJoin, and DisjunctJoin), the missing side's property is null.

## NOTES

- String key comparisons use `OrdinalIgnoreCase` (case-insensitive) by default. Use `-DefaultStringComparer [StringComparer]::Ordinal` to make all string key comparisons case-sensitive, or use `-Comparer` to override the comparer for specific key fields only.
- For all keyed join types, the entire right collection is loaded into memory (materialized) before processing begins. For very large right collections, this has memory implications.
- Duplicate keys in the right collection cause a non-terminating error by default. Use `-KeyCollisionPreference` to change this to `Warn`, `Ignore`, `Group`, or `GroupThenFlatten` depending on how duplicates should be handled.
- `-LeftKey` and `-RightKey` must always have the same number of elements. A mismatch produces a non-terminating error and no output is written.
- Property names in the `-Comparer` hashtable are matched case-insensitively against the key field names.
- `-CrossJoin` and `-ZipJoin` do not use or accept any key parameters.

## RELATED LINKS

[ConvertTo-Hashtable](ConvertTo-Hashtable.md)
