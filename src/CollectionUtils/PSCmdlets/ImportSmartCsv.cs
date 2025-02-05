﻿using System.IO;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsData.Import, PSCmdletNouns.SmartCsv)]
  [OutputType(typeof(PSObject[]))]
  public sealed class ImportSmartCsv : PSCmdlet
  {
    #region Parameters

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(Path),
      Position = 1,
      ValueFromPipeline = true)]
    public string? Path { get; set; }

    #endregion

    private CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();
    private CsvService _CsvService = new CsvService();

    protected override void ProcessRecord()
    {
      var csvInput = File.ReadAllText(Path!);

      if (_CancellationTokenSource.IsCancellationRequested)
        return;

      foreach (var psObject in _CsvService.ParseCsvInput(csvInput, _CancellationTokenSource.Token))
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
