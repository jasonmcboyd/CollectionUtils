# CollectionUtils Bug Fix List

Bugs identified during code review. Organized by severity.

---

## Critical

- [ ] **#1 — SheetIndex filtering broken (index not incremented for skipped sheets)**
  - `src/CollectionUtils/Data/ExcelService.cs:46-84`
  - The `index` variable is only incremented after `yield return` (line 84). When a sheet is skipped via `continue` (line 56), `index` never advances. `Import-SmartExcel -SheetIndex 1` returns nothing — only index 0 ever works.
  - **Fix:** Increment `index` unconditionally at the end of each loop iteration, before the `continue`.

- [x] **#2 — Manifest exports `ConvertFrom-SmartExcel` instead of `Import-SmartExcel`**
  - `scripts/create-module-manifest.ps1:56`
  - The `$functionsToExport` array contains `'ConvertFrom-SmartExcel'`, but the actual cmdlet is `Import-SmartExcel`. The Excel import feature is invisible to PSGallery users.
  - **Fix:** Change `'ConvertFrom-SmartExcel'` to `'Import-SmartExcel'` in the exports list.

- [ ] **#3 — CI/CD publishes to PSGallery on every push to every branch**
  - `.github/workflows/publish-release.yml:3`
  - Triggers on `on: [push]` with no branch or tag filter. Every push (including feature branches and WIP) publishes a new version.
  - **Fix:** Restrict to tag pushes (e.g., `on: push: tags: ['v*']`) or add a branch filter and environment gate.

---

## High

- [ ] **#4 — Relative paths resolve against .NET working directory, not PowerShell `$PWD`**
  - `src/CollectionUtils/PSCmdlets/ImportSmartCsv.cs:30`
  - `src/CollectionUtils/PSCmdlets/ImportSmartExcel.cs:50` (via ExcelService)
  - After `Set-Location`, PowerShell `$PWD` and .NET `Environment.CurrentDirectory` diverge. Relative paths like `.\data.csv` open the wrong file or throw "file not found."
  - **Fix:** Resolve paths via `SessionState.Path.GetUnresolvedProviderPathFromPSPath(Path)` before passing to file APIs.

- [ ] **#5 — NullReferenceException on null Excel header cells**
  - `src/CollectionUtils/Data/ExcelService.cs:66`
  - `reader[columnIndex].ToString()` throws when a header cell is empty/null. Empty trailing columns are extremely common in Excel files.
  - **Fix:** Use `reader[columnIndex]?.ToString() ?? $"Column{columnIndex}"` or similar null-safe logic.

- [ ] **#6 — `reader.Read()` return value not checked — crashes on empty worksheets**
  - `src/CollectionUtils/Data/ExcelService.cs:61`
  - `reader.Read()` returns `false` for empty sheets but the result is discarded. Subsequent `reader[columnIndex]` access throws.
  - **Fix:** Check the return value and `continue` to the next sheet if `false`.

- [ ] **#7 — Excel file opened without `FileShare.Read`**
  - `src/CollectionUtils/Data/ExcelService.cs:43`
  - `File.Open` defaults to `FileShare.None`, locking the file exclusively. Cannot import files currently open in Excel.
  - **Fix:** Use `new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)` as the CSV importer already does.

- [ ] **#8 — NullReferenceException when PSObject is missing a key property**
  - `src/CollectionUtils/PropertyGetter.cs:67-69`
  - `obj.Properties[propertyName]` returns null for missing properties, then `.Value` throws. Has a TODO acknowledging the issue.
  - **Fix:** Add a null check and throw a descriptive `PropertyResolutionException` or `NullKeyException`.

- [ ] **#9 — Thread-unsafe static dictionary in PropertyGetter**
  - `src/CollectionUtils/PropertyGetter.cs:78`
  - `Dictionary<(string,string), PropertyInfo>` mutated via `GetOrAdd` without synchronization. Corrupts in multi-runspace scenarios (`ForEach-Object -Parallel`).
  - **Fix:** Replace with `ConcurrentDictionary<TKey, TValue>`.

- [ ] **#10 — `KeyCollisionPreference.Error` does not stop processing**
  - `src/CollectionUtils/PSCmdletKeyCollisionStrategySelector.cs:16-28`
  - Calls `WriteError` (non-terminating) but cancellation is commented out with a TODO. Duplicate keys are silently dropped. Behaves identically to `Ignore`.
  - **Fix:** Uncomment and implement the cancellation, or use `ThrowTerminatingError`.

- [ ] **#11 — `DisjunctJoin` positional parameter conflict (`LeftKey` and `RightKey` both at Position 4)**
  - `src/CollectionUtils/PSCmdlets/JoinCollectionPsCmdlet.cs:141-144`
  - `RightKey` for `DisjunctJoin` is `Position = 4`, same as `LeftKey`. All other join types correctly use Position 5 for `RightKey`.
  - **Fix:** Change `Position = 4` to `Position = 5` for the `DisjunctJoin|LeftKey|RightKey` parameter set.

- [ ] **#12 — `ValidateKeyFields` incorrectly requires matching property names between `LeftKey`/`RightKey`**
  - `src/CollectionUtils/PSCmdlets/JoinCollectionPsCmdlet.cs:226-246`
  - Rejects the primary use case for separate `LeftKey`/`RightKey` — joining on different property names (e.g., `EmployeeId` vs `EmpId`).
  - **Fix:** Only validate that `LeftKey` and `RightKey` have the same count. Remove the property name matching loop.

- [ ] **#13 — Validation errors don't prevent continued processing**
  - `src/CollectionUtils/PSCmdlets/ConvertToHashtablePsCmdlet.cs:45-85`
  - `src/CollectionUtils/PSCmdlets/JoinCollectionPsCmdlet.cs:321-331`
  - Both cmdlets call `WriteError` + `Cancel()` on validation failure, but nothing checks the cancellation state. Processing continues and produces (potentially wrong) output.
  - **Fix:** Check `_CancellationTokenSource.IsCancellationRequested` after validation and return early, or use `ThrowTerminatingError`.

---

## Medium

- [ ] **#14 — Culture-sensitive parsing in type inference and conversion**
  - `src/CollectionUtils/TypeConverter.cs:43-48`
  - `src/CollectionUtils/Data/DataColumnsService.cs:84,90,102`
  - All `Parse`/`TryParse` calls use the current thread's `CultureInfo`. Data from different locales is silently misinterpreted (e.g., decimal separators, date formats).
  - **Fix:** Use `CultureInfo.InvariantCulture` for parsing, or accept a `-Culture` parameter.

- [ ] **#15 — `ReadFields()` return value not null-checked in CsvService**
  - `src/CollectionUtils/Data/CsvService.cs:35,43`
  - `CsvTextFieldParser.ReadFields()` can return null at end-of-data. Subsequent `.Select()` or `.Length` throws on malformed CSV files.
  - **Fix:** Add null checks after `ReadFields()` calls.

- [ ] **#16 — `CancellationTokenSource` never disposed in 4 cmdlets**
  - `src/CollectionUtils/PSCmdlets/ConvertFromSmartCsv.cs:22`
  - `src/CollectionUtils/PSCmdlets/ImportSmartCsv.cs:23`
  - `src/CollectionUtils/PSCmdlets/ImportSmartExcel.cs:45`
  - `src/CollectionUtils/PSCmdlets/ConvertToHashtablePsCmdlet.cs:39` (Dispose exists but skips token source)
  - **Fix:** Implement `IDisposable` and dispose the `CancellationTokenSource`, or dispose in `EndProcessing`/`StopProcessing`.

- [ ] **#17 — ScriptBlock result assumed non-empty in PropertyGetter**
  - `src/CollectionUtils/PropertyGetter.cs:57`
  - `resultsFromScriptBlock[0]` with no empty check. Key expressions like `{ }` crash with `IndexOutOfRangeException`.
  - **Fix:** Check for empty results and throw a descriptive error.

- [ ] **#18 — XOR hash combination in `HashtableStructuralEqualityComparer`**
  - `src/CollectionUtils/HashtableStructuralEqualityComparer.cs:64-83`
  - XOR is commutative and self-cancelling. Composite keys with identical or swapped field values collide, degrading performance to O(n).
  - **Fix:** Use `HashCode.Combine` (already used in `PropertyGetter.TupleComparer`).

- [ ] **#19 — Case-sensitive comparer validation in `ConvertToHashtable`**
  - `src/CollectionUtils/PSCmdlets/ConvertToHashtablePsCmdlet.cs:52`
  - Uses `==` (case-sensitive) to match comparer keys. `Join-Collection` correctly uses `StringComparison.OrdinalIgnoreCase`.
  - **Fix:** Use `.Equals(keyComparer.Key, StringComparison.OrdinalIgnoreCase)`.

- [x] **#20 — `Convert-Property` aborts entire pipeline on single conversion failure**
  - `src/CollectionUtils/PSCmdlets/ConvertProperty.cs:54-59`
  - `TypeConverter.Parse`/`Convert` throws unhandled `FormatException`, `OverflowException`, etc. One bad value kills the whole pipeline.
  - **Fix:** Wrap in try-catch, use `WriteError` (non-terminating), and continue processing.

- [x] **#21 — `$null | ConvertFrom-SmartCsv` causes unhandled NullReferenceException**
  - `src/CollectionUtils/PSCmdlets/ConvertFromSmartCsv.cs:27`
  - `CsvInput` is nullable but accessed with null-forgiving `CsvInput!`. Piping `$null` crashes inside `CsvService`.
  - **Fix:** Add a null guard in `ProcessRecord` before calling `ParseCsvInput`.

- [x] **#22 — `TestCollectionPSCmdlet` calls `StopProcessing()` from `ProcessRecord()`**
  - `src/CollectionUtils/PSCmdlets/TestCollectionPSCmdlet.cs:94`
  - Misuse of the lifecycle API. Short-circuit logic produces correct results, but the upstream pipeline continues sending objects unnecessarily.
  - **Fix:** Throw `PipelineStoppedException` to properly halt the pipeline, or set a flag and skip remaining input without calling `StopProcessing()`.

---

## Low

- [x] **#23 — CSV error message reports wrong line number**
  - `src/CollectionUtils/Data/CsvService.cs:39-48`
  - `rowCount` starts at 1 for the first data row, but that's actually line 2 of the file (after the header). Error message points users to the wrong line.
  - **Fix:** Use `rowCount + 1` for the file line number, or say "data row" instead of "line".

- [x] **#24 — `"1"`/`"0"` boolean handling in `TypeConverter.Parse` is unreachable via inference**
  - `src/CollectionUtils/TypeConverter.cs:29-34`
  - `src/CollectionUtils/Data/DataColumnsService.cs:96`
  - `bool.TryParse("1")` returns `false` in .NET, so inference never classifies such columns as boolean. The special-case code in `Parse` is dead code.
  - **Fix:** Either remove the `"1"`/`"0"` handling from `Parse`, or add `"1"`/`"0"` detection to the inference logic.

- [x] **#25 — `update-docs.ps1` hardcodes `net6.0` in DLL path**
  - `scripts/update-docs.ps1:8`
  - If the target framework changes, the docs script silently breaks.
  - **Fix:** Read the target framework from the `.csproj` or use a glob pattern.

- [x] **#26 — Empty hashtable returned with wrong comparer**
  - `src/CollectionUtils/HashtableBuilderBase.cs:102-105`
  - Developer-acknowledged TODO. When no objects are added, `new Hashtable()` uses the default comparer instead of the configured one.
  - **Fix:** Pass the configured comparer to the `Hashtable` constructor.
