using CollectionUtils.Data;
using System;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsData.Import, PSCmdletNouns.SmartExcel, DefaultParameterSetName = nameof(Path))]
  [OutputType(typeof(PSObject[]))]
  public sealed class ImportSmartExcel : PSCmdlet
  {
    #region Parameters

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(Path),
      Position = 0,
      ValueFromPipeline = true)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(SheetName),
      Position = 0,
      ValueFromPipeline = true)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(SheetIndex),
      Position = 0,
      ValueFromPipeline = true)]
    public string? Path { get; set; }

    [Parameter(
      Mandatory = false,
      ParameterSetName = nameof(SheetName),
      Position = 1)]
    public string[]? SheetName { get; set; }

    [Parameter(
      Mandatory = false,
      ParameterSetName = nameof(SheetIndex),
      Position = 1)]
    public int[]? SheetIndex { get; set; }

    #endregion

    private CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();
    private ExcelService _ExcelService = new ExcelService();

    protected override void ProcessRecord()
    {
      foreach (var psObject in _ExcelService.ReadExcelWorkbook(Path!, SheetName ?? Array.Empty<string>(), SheetIndex ?? Array.Empty<int>(), _CancellationTokenSource.Token))
        WriteObject(psObject);

      base.ProcessRecord();
    }

    protected override void StopProcessing()
    {
      _CancellationTokenSource.Cancel();

      base.StopProcessing();
    }
  }
}
