using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.Data
{
  internal class ExcelService
  {
    public IEnumerable<PSObject> ReadExcelWorkbook(
      string workbookPath,
      string[] worksheetNames,
      int[] worksheetIndexes,
      CancellationToken cancellationToken)
    {
      var worksheetDataColumns = ConvertExcelWorkbookToDataColumns(workbookPath, worksheetNames, worksheetIndexes, cancellationToken);

      if (cancellationToken.IsCancellationRequested)
        yield break;

      foreach (var worksheet in worksheetDataColumns)
      {
        var data = DataColumnsService.ConvertDataColumnsToPsObject(worksheet.DataColumns, cancellationToken).ToArray();

        var psObject = new PSObject();

        psObject.Properties.Add(new PSNoteProperty("WorksheetName", worksheet.WorksheetName));
        psObject.Properties.Add(new PSNoteProperty("Data", data));

        yield return psObject;
      }
    }


    private IEnumerable<WorksheetDataColumns> ConvertExcelWorkbookToDataColumns(
      string workbookPath,
      string[] worksheetNames,
      int[] worksheetIndexes,
      CancellationToken cancellationToken)
    {
      using var stream = File.Open(workbookPath, FileMode.Open, FileAccess.Read);
      using var reader = ExcelReaderFactory.CreateReader(stream);

      var index = 0;

      do
      {
        if (worksheetNames.Length > 0 || worksheetIndexes.Length > 0)
        {
          var matchedWorksheetName = worksheetNames.Any(worksheetName => worksheetName.Equals(reader.Name, StringComparison.OrdinalIgnoreCase));
          var matchedWorksheetIndex = worksheetIndexes.Any(worksheetIndex => worksheetIndex == index);

          if (!matchedWorksheetIndex && !matchedWorksheetName)
            continue;
        }

        var columnCount = reader.FieldCount;

        reader.Read();

        var columns =
          Enumerable
          .Range(0, columnCount)
          .Select(columnIndex => reader[columnIndex].ToString())
          .Select(columnName => new DataColumn(columnName)).ToArray();

        while (reader.Read())
        {
          if (cancellationToken.IsCancellationRequested)
            throw new OperationCanceledException();

          for (int i = 0; i < columns.Length; i++)
            columns[i].Values.Add(reader[i]?.ToString());
        }

        yield return new WorksheetDataColumns
        {
          WorksheetName = reader.Name,
          DataColumns = columns
        };

        index++;
      } while (reader.NextResult());
    }

    private class WorksheetDataColumns
    {
      public string WorksheetName { get; set; }
      public DataColumn[] DataColumns { get; set; }
    }
  }
}
