using System.Management.Automation;
using System.Threading;
using CollectionUtils.Data;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsData.ConvertFrom, PSCmdletNouns.SmartCsv)]
  [OutputType(typeof(PSObject[]))]
  public sealed class ConvertFromSmartCsv : PSCmdlet
  {
    #region Parameters

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(CsvInput),
      Position = 1,
      ValueFromPipeline = true)]
    public string? CsvInput { get; set; }

    #endregion

    private CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();
    private CsvService _CsvService = new CsvService();

    protected override void ProcessRecord()
    {
      foreach (var psObject in _CsvService.ParseCsvInput(CsvInput!, _CancellationTokenSource.Token))
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
