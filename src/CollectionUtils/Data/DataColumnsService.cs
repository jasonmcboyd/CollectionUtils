using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.Data
{
  internal static class DataColumnsService
  {
    public static IEnumerable<PSObject> ConvertDataColumnsToPsObject(
      DataColumn[] dataColumns,
      CancellationToken cancellationToken)
    {
      if (dataColumns.Length == 0)
        yield break;

      var rowCount = dataColumns[0].Values.Count;

      if (rowCount == 0)
        yield break;

      var typeCodes = DataColumnsService.InferTypeCodesForRawCsvColumns(dataColumns);

      for (int row = 0; row < rowCount; row++)
      {
        var psObject = new PSObject();

        for (int column = 0; column < typeCodes.Length; column++)
        {
          var columnName = dataColumns[column].ColumnName;
          var rawValue = dataColumns[column].Values[row];
          var parsedValue = TypeConverter.Parse(rawValue, typeCodes[column]);

          psObject.Properties.Add(new PSNoteProperty(columnName, parsedValue));
        }

        yield return psObject;

        if (cancellationToken.IsCancellationRequested)
          throw new OperationCanceledException();
      }
    }
    public static TypeCode[] InferTypeCodesForRawCsvColumns(DataColumn[] dataColumns)
    {
      return
        dataColumns
        .Select(InferTypeCodeFromCollection)
        .ToArray();
    }

    public static TypeCode InferTypeCodeFromCollection(DataColumn dataColumns)
    {
      var matchedBoolean = false;
      var matchedInteger = false;
      var matchedDecimal = false;
      var matchedDateTime = false;
      var matchedString = false;

      var matchedNull = false;
      var matchedBlank = false;

      foreach (var value in dataColumns.Values)
      {
        if (value == null || value.Equals("null", StringComparison.OrdinalIgnoreCase))
        {
          matchedNull = true;
          continue;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
          matchedBlank = true;
          continue;
        }

        // Order matters here. If it successfully parses the integer, then
        // it will also successfully parse as a decimal. By testing the
        // integer first we can preempt the decimal test if it matches.
        // If at the end of the test we have matched on the integer type
        // but never the decimal type, then we know all values are integer.
        // If we have matched on integer or decimal, then we know at least
        // one value was not a whole number so we should treat all values
        // as decimals.
        if (int.TryParse(value, out var _))
        {
          matchedInteger = true;
          continue;
        }

        if (decimal.TryParse(value, out var _))
        {
          matchedDecimal = true;
          continue;
        }

        if (bool.TryParse(value, out var _))
        {
          matchedBoolean = true;
          continue;
        }

        if (DateTime.TryParse(value, out var _))
        {
          matchedDateTime = true;
          continue;
        }

        matchedString = true;
        break;
      }

      if (matchedString || !matchedInteger && !matchedDateTime && !matchedDecimal && !matchedBoolean)
        return TypeCode.String;

      var matchCount = 0;

      if (matchedDateTime)
        matchCount++;

      if (matchedBoolean)
        matchCount++;

      if (matchedInteger || matchedDecimal)
        matchCount++;

      if (matchCount != 1)
        throw new InvalidOperationException($"Unable to infer the datatype for '{dataColumns.ColumnName}' because it matched on multiple types.");

      // Order matters here. We must test decimal before we test integer.
      if (matchedDecimal)
        return TypeCode.Decimal;

      if (matchedInteger)
        return TypeCode.Int32;

      if (matchedDateTime)
        return TypeCode.DateTime;

      if (matchedBoolean)
        return TypeCode.Boolean;

      throw new InvalidOperationException("Unhandled type encountered.");
    }

  }
}
