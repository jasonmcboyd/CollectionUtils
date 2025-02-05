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

      var valueConverters = CreateValueConverters(rawCsvColumns).ToArray();

      for (int row = 0; row < rowCount; row++)
      {
        var psObject = new PSObject();

        for (int column = 0; column < valueConverters.Length; column++)
        {
          var columnName = rawCsvColumns[column].ColumnName;
          var rawValue = rawCsvColumns[column].Values[row];
          var parsedValue = valueConverters[column].ConvertValue(rawValue);

          psObject.Properties.Add(new PSNoteProperty(columnName, parsedValue));
        }

        yield return psObject;

        if (cancellationToken.IsCancellationRequested)
          yield break;
      }
    }

    private ValueConverter[] CreateValueConverters(RawCsvColumn[] rawCsvColumns)
    {
      return
        rawCsvColumns
        .Select(column => InferValueDefinitionFromCollection(column.Values))
        .Select(valueType => new ValueConverter(valueType))
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

    private ValueDefinition InferValueDefinitionFromCollection(IEnumerable<string?> values)
    {
      var isBoolean = true;
      var isInteger = true;
      var isDecimal = true;
      var isDateTime = true;

      var hasNull = false;
      var hasBlank = false;

      foreach (var value in values)
      {
        if (value is null || value.Equals("null", StringComparison.OrdinalIgnoreCase))
        {
          hasNull = true;
          continue;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
          hasBlank = true;
          continue;
        }

        if (isBoolean && !bool.TryParse(value, out var _))
          isBoolean = false;

        if (isInteger && !int.TryParse(value, out var _))
          isInteger = false;

        if (isDecimal && !decimal.TryParse(value, out var _))
          isDecimal = false;

        if (isDateTime && !DateTime.TryParse(value, out var _))
          isDateTime = false;

        if (!isBoolean && !isInteger && !isDecimal && !isDateTime)
          break;
      }

      if (!isBoolean && !isInteger && !isDecimal && !isDateTime)
        return new ValueDefinition(ValueType.String, hasNull);

      var matchCount = 0;

      if (isDateTime)
        matchCount++;

      if (isBoolean)
        matchCount++;

      if (isInteger || isDecimal)
        matchCount++;

      if (matchCount != 1)
        throw new InvalidOperationException("Unable to infer the datatype because it matched on multiple possible types.");

      // Order matters because if the value is an integer is will also be a decimal.
      if (isInteger)
        return new ValueDefinition(ValueType.Integer, hasNull || hasBlank);

      if (isDecimal)
        return new ValueDefinition(ValueType.Decimal, hasNull || hasBlank);

      if (isDateTime)
        return new ValueDefinition(ValueType.DateTime, hasNull || hasBlank);

      if (isBoolean)
        return new ValueDefinition(ValueType.Boolean, hasNull || hasBlank);

      throw new InvalidOperationException("Unhandled type encountered.");
    }

    private class ValueConverter
    {
      public ValueConverter(ValueDefinition valueDefinition)
      {
        ValueDefinition = valueDefinition;
      }

      private ValueDefinition ValueDefinition { get; }

      public object? ConvertValue(string? value)
      {
        if (value == null
            || value.Equals("null", StringComparison.OrdinalIgnoreCase)
            || (ValueDefinition.ValueType != ValueType.String && string.IsNullOrWhiteSpace(value)))
        {
          return null;
        }

        switch (ValueDefinition.ValueType)
        {
          case ValueType.Boolean:
              return bool.Parse(value);

          case ValueType.DateTime:
              return DateTime.Parse(value);

          case ValueType.Decimal:
              return decimal.Parse(value);

          case ValueType.Integer:
              return int.Parse(value);

          case ValueType.String:
              return value;

          default:
            throw new NotImplementedException($"Conversion of value type '{ValueDefinition}' has not been implemented.");
        }
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

    private enum ValueType
    {
      Boolean,
      Decimal,
      DateTime,
      Integer,
      String
    }

    private struct ValueDefinition
    {
      public ValueDefinition(ValueType valueType, bool isNullable)
      {
        ValueType = valueType;
        IsNullable = isNullable;

        switch (valueType)
        {
          case ValueType.Boolean:
            Type = IsNullable ? typeof(bool?) : typeof(bool);
            break;
          case ValueType.DateTime:
            Type = IsNullable ? typeof(DateTime?) : typeof(DateTime);
            break;
          case ValueType.Decimal:
            Type = IsNullable ? typeof(decimal?) : typeof(decimal);
            break;
          case ValueType.Integer:
            Type = IsNullable ? typeof(int?) : typeof(int);
            break;
          case ValueType.String:
            Type = typeof(string);
            break;
          default:
            throw new NotImplementedException($"Value type '{ValueType}' has not been implemented.");
        }
      }

      public ValueType ValueType { get; }
      public bool IsNullable { get; }
      public Type Type { get; }
    }
  }
}
