using System;
using System.Globalization;

namespace CollectionUtils
{
  internal static class TypeConverter
  {
    public static object? Convert(object? value, TypeCode type)
    {
      if (value == null || value == DBNull.Value)
        return null;

      if (value is not IConvertible)
        throw new InvalidOperationException($"Cannot convert type '{value.GetType().FullName}'.");

      return System.Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
    }

    public static object? Parse(string? value, TypeCode type)
    {
      if (string.IsNullOrWhiteSpace(value)
          || value.Equals("null", StringComparison.OrdinalIgnoreCase))
      {
        return null;
      }

      switch (type)
      {
        case TypeCode.Boolean:
          return value switch
          {
            "1" => true,
            "0" => false,
            _ => bool.Parse(value)
          };

        case TypeCode.Byte:
          return Byte.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.Char:
          return Char.Parse(value);

        case TypeCode.DateTime:
          return DateTime.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.Decimal:
          return Decimal.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.Double:
          return Double.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.Int16:
          return Int16.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.Int32:
          return Int32.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.Int64:
          return Int64.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.Object:
          return value;

        case TypeCode.SByte:
          return SByte.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.Single:
          return Single.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.String:
          return value;

        case TypeCode.UInt16:
          return UInt16.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.UInt32:
          return UInt32.Parse(value, CultureInfo.InvariantCulture);

        case TypeCode.UInt64:
          return UInt64.Parse(value, CultureInfo.InvariantCulture);

        default:
          throw new NotSupportedException($"Type code '{type}' is not supported.");
      }
    }
  }
}
