using NotVisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils
{
  internal class CsvService
  {
    public IEnumerable<PSObject> ParseCsvInput(
      string csvInput,
      CancellationToken cancellationToken)
    {
      var rawCsvColumns = ParseRawCsvColumns(csvInput);

      if (cancellationToken.IsCancellationRequested)
        yield break;

      var rowCount = rawCsvColumns == null ? 0 : rawCsvColumns[0].Values.Count;

      if (rowCount == 0)
        yield break;

      var typeCodes = InferTypeCodesForRawCsvColumns(rawCsvColumns);

      var valueConverter = new ValueConverter();

      for (int row = 0; row < rowCount; row++)
      {
        var psObject = new PSObject();

        for (int column = 0; column < typeCodes.Length; column++)
        {
          var columnName = rawCsvColumns[column].ColumnName;
          var rawValue = rawCsvColumns[column].Values[row];
          var parsedValue = valueConverter.ConvertValue(rawValue, typeCodes[column]);

          psObject.Properties.Add(new PSNoteProperty(columnName, parsedValue));
        }

        yield return psObject;

        if (cancellationToken.IsCancellationRequested)
          yield break;
      }
    }

    private TypeCode[] InferTypeCodesForRawCsvColumns(RawCsvColumn[] rawCsvColumns)
    {
      return
        rawCsvColumns
        .Select(InferTypeCodeFromCollection)
        .ToArray();
    }

    private RawCsvColumn[]? ParseRawCsvColumns(string csvInput)
    {
      using var csvReader = new StringReader(csvInput);
      using var parser = new CsvTextFieldParser(csvReader);

      if (parser.EndOfData)
        return null;

      var rawCsvColumns =
        parser
        .ReadFields()
        .Select(field => new RawCsvColumn(field.Trim()))
        .ToArray();

      var rowCount = 0;

      while (!parser.EndOfData)
      {
        string[] fields = parser.ReadFields();
        rowCount++;

        if (fields.Length != rawCsvColumns.Length)
          throw new InvalidOperationException(
            $"Encountered a row at line {rowCount} that did not have the same number of fields as headers.");

        for (int i = 0; i < fields.Length; i++)
          rawCsvColumns[i].Values.Add(fields[i]);
      }

      return rawCsvColumns;
    }

    private TypeCode InferTypeCodeFromCollection(RawCsvColumn rawCsvColumn)
    {
      var matchedBoolean = false;
      var matchedInteger = false;
      var matchedDecimal = false;
      var matchedDateTime = false;
      var matchedString = false;

      var matchedNull = false;
      var matchedBlank = false;

      foreach (var value in rawCsvColumn.Values)
      {
        if (value.Equals("null", StringComparison.OrdinalIgnoreCase))
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

      if (matchedString || (!matchedInteger && !matchedDateTime && !matchedDecimal && !matchedBoolean))
        return TypeCode.String;

      var matchCount = 0;

      if (matchedDateTime)
        matchCount++;

      if (matchedBoolean)
        matchCount++;

      if (matchedInteger || matchedDecimal)
        matchCount++;

      if (matchCount != 1)
        throw new InvalidOperationException($"Unable to infer the datatype for '{rawCsvColumn.ColumnName}' because it matched on multiple types.");

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

    private class ValueConverter
    {
      public object? ConvertValue(string? value, TypeCode typeCode)
      {
        if (value == null
            || value.Equals("null", StringComparison.OrdinalIgnoreCase)
            || (typeCode != TypeCode.String && string.IsNullOrWhiteSpace(value)))
        {
          return null;
        }

        return TypeConverter.Parse(value, typeCode);
      }
    }

    private class RawCsvColumn
    {
      public RawCsvColumn(string columnName)
      {
        ColumnName = columnName;
      }

      public string ColumnName { get; set; }
      public List<string> Values { get; } = new();
    }
  }
}
