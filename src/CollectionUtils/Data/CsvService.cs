using NotVisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.Data
{
  internal class CsvService
  {
    public IEnumerable<PSObject> ParseCsvInput(
      string csvInput,
      CancellationToken cancellationToken)
    {
      var dataColumns = ConvertCsvStringToDataColumns(csvInput);

      if (cancellationToken.IsCancellationRequested)
        return Enumerable.Empty<PSObject>();

      return DataColumnsService.ConvertDataColumnsToPsObject(dataColumns, cancellationToken);
    }


    private DataColumn[] ConvertCsvStringToDataColumns(string csvInput)
    {
      using var csvReader = new StringReader(csvInput);
      using var parser = new CsvTextFieldParser(csvReader);

      if (parser.EndOfData)
        return Array.Empty<DataColumn>();

      var dataColumns =
        parser
        .ReadFields()
        .Select(field => new DataColumn(field.Trim()))
        .ToArray();

      var rowCount = 0;

      while (!parser.EndOfData)
      {
        string[] fields = parser.ReadFields();
        rowCount++;

        if (fields.Length != dataColumns.Length)
          throw new InvalidOperationException(
            $"Encountered a row at line {rowCount} that did not have the same number of fields as headers.");

        for (int i = 0; i < fields.Length; i++)
          dataColumns[i].Values.Add(fields[i]);
      }

      return dataColumns;
    }
  }
}
