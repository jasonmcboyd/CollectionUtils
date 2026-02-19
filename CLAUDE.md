# CollectionUtils

A binary PowerShell module written in C# (.NET 6.0) that provides utilities for working with collections. Published to the PowerShell Gallery.

**Author:** Jason Boyd
**Repository:** https://github.com/jasonmcboyd/CollectionUtils
**License:** MIT

## Project Structure

```
CollectionUtils/
├── src/
│   ├── CollectionUtils/              # Main C# project (compiles to DLL)
│   │   ├── PSCmdlets/                # Cmdlet implementations
│   │   ├── Data/                     # CSV/Excel parsing and type inference
│   │   ├── JoinCommandHandlers/      # Join strategy implementations
│   │   ├── Utilities/                # Memory pooling, extensions, helpers
│   │   └── CollectionUtils.csproj
│   ├── CollectionUtils.Test/         # MSTest unit tests
│   └── CollectionUtils.sln
├── docs/                             # PowerShell help documentation (markdown)
├── scripts/                          # Build and manifest generation scripts
├── .github/workflows/                # CI/CD (GitHub Actions)
└── sampleData/                       # Sample data files for testing
```

## Architecture

This is a **binary module** — no .psm1 file. All cmdlets are C# classes inheriting from `PSCmdlet`, compiled into `CollectionUtils.dll` which serves as the RootModule. PowerShell discovers cmdlets via `[Cmdlet]` attributes.

Key patterns:
- **Command Handler pattern** for join strategies (`IJoinCommandHandler` with Cross/Zip/Keyed implementations)
- **Strategy pattern** for key collision handling (Error, Group, Ignore, Warn)
- **Object pooling** (`SharedListPool`, `SharedArrayPool`) to reduce GC pressure
- **Type inference** for CSV/Excel imports (Boolean → Integer → Decimal → DateTime → String)

## Exported Cmdlets (8)

| Cmdlet | Purpose |
|--------|---------|
| `Join-Collection` | SQL-style joins (Inner, Left, Right, Outer, Cross, Zip, Disjunct) between two collections |
| `ConvertTo-Hashtable` | Converts PSObject collection to Hashtable indexed by key(s) |
| `Test-Collection` | Tests if Any/All items satisfy a predicate (short-circuits) |
| `ConvertFrom-DataTable` | Converts DataTable/DataRow to PSObject array |
| `ConvertFrom-SmartCsv` | Parses CSV string with automatic type inference |
| `Import-SmartCsv` | Imports CSV file with automatic type inference |
| `Import-SmartExcel` | Imports Excel workbook with sheet filtering and type inference |
| `Convert-Property` | Converts a named property to a specified TypeCode |

## Building

```powershell
dotnet build ./src --configuration Release
dotnet test ./src --configuration Release
dotnet publish ./src/CollectionUtils --configuration Release --output ./publish/CollectionUtils --no-self-contained
./scripts/create-module-manifest.ps1 -PatchVersion 1
```

## Dependencies

- **PowerShellStandard.Library 5.1.1** — PowerShell SDK
- **CsvTextFieldParser 1.2.2** — CSV parsing
- **ExcelDataReader 3.7.0** — Excel file reading
- **Microsoft.Extensions.ObjectPool 7.0.10** — Memory pooling

## Testing

Tests use **MSTest** and run PowerShell commands in-process via `Microsoft.PowerShell.SDK`. Test command builders provide a fluent API for constructing cmdlet invocations. Run with `dotnet test ./src`.

## CI/CD

GitHub Actions workflow (`.github/workflows/publish-release.yml`) runs on tag pushes matching `v*`: build → test → publish DLL → generate manifest → publish to PSGallery. Module version is derived from the tag (e.g., tag `v0.1.0-alpha` publishes version `0.1.0-alpha`).

## Key Source Files

- Cmdlet implementations: `src/CollectionUtils/PSCmdlets/`
- Join logic: `src/CollectionUtils/JoinCommandHandlers/`
- Type inference: `src/CollectionUtils/Data/DataColumnsService.cs`
- Property access (PSObject/Hashtable/DataRow/reflection): `src/CollectionUtils/PropertyGetter.cs`
- Type conversion: `src/CollectionUtils/TypeConverter.cs`
- Manifest script: `scripts/create-module-manifest.ps1`
