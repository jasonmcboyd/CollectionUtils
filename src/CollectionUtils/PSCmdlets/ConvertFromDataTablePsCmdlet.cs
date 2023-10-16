using System.Data;
using System.Management.Automation;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsData.ConvertFrom, PSCmdletNouns.DataTable)]
  [OutputType(typeof(PSObject[]))]
  public sealed class ConvertFromDataTablePSCmdlet : PSCmdlet
  {
    #region Parameters

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(Table),
      Position = 1,
      ValueFromPipeline = true)]
    public DataTable[]? Table { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(Row),
      Position = 1,
      ValueFromPipeline = true)]
    public DataRow[]? Row { get; set; }

    #endregion

    private bool _ShouldStop = false;

    protected override void ProcessRecord()
    {
      if (Table is not null)
        for (int i = 0; i < Table.Length; i++)
          ProcessDataTable(Table[i]);

      if (Row is not null)
        ProcessDataRowArray(Row);

      base.ProcessRecord();
    }

    private void ProcessDataTable(DataTable table) => ProcessDataRowCollection(table.Rows);

    private void ProcessDataRowCollection(DataRowCollection rows)
    {
      for (int i = 0; i < rows.Count; i++)
      {
        if (_ShouldStop)
          return;

        var row = rows[i];
        ProcessDataRow(row);
      }
    }

    private void ProcessDataRowArray(DataRow[] rows)
    {
      for (int i = 0; i < rows.Length; i++)
      {
        if (_ShouldStop)
          return;

        var row = rows[i];
        ProcessDataRow(row);
      }
    }

    private void ProcessDataRow(DataRow row)
    {
      var table = row.Table;
      var columns = table.Columns;

      var psObject = new PSObject();

      for (int i = 0; i < columns.Count; i++)
      {
        var column = columns[i];
        var columnName = column.ColumnName;
        var columnValue = row[columnName];

        psObject.Properties.Add(new PSNoteProperty(columnName, columnValue));
      }

      WriteObject(psObject);
    }

    protected override void StopProcessing()
    {
      _ShouldStop = true;

      base.StopProcessing();
    }
  }
}
