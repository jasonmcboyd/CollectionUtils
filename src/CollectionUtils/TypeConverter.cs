using System;

namespace CollectionUtils
{
  internal static class TypeConverter
  {
    public static object? Convert(object? value, TypeCode type)
    {
      if (value == null)
        return null;

      if (value is not IConvertible)
        throw new InvalidOperationException($"Cannot convert type '{value.GetType().FullName}'.");

      return System.Convert.ChangeType(value, type);
    }

    public static object? Parse(string? value, TypeCode type)
    {
      if (string.IsNullOrWhiteSpace(value))
        return null;

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
          return Byte.Parse(value);

        case TypeCode.Char:
          return Char.Parse(value);

        case TypeCode.DateTime:
          return DateTime.Parse(value);

        case TypeCode.Decimal:
          return Decimal.Parse(value);

        case TypeCode.Double:
          return Double.Parse(value);

        case TypeCode.Int16:
          return Int16.Parse(value);

        case TypeCode.Int32:
          return Int32.Parse(value);

        case TypeCode.Int64:
          return Int64.Parse(value);

        case TypeCode.Object:
          return value;

        case TypeCode.SByte:
          return SByte.Parse(value);

        case TypeCode.Single:
          return Single.Parse(value);

        case TypeCode.String:
          return value;

        case TypeCode.UInt16:
          return UInt16.Parse(value);

        case TypeCode.UInt32:
          return UInt32.Parse(value);

        case TypeCode.UInt64:
          return UInt64.Parse(value);

        default:
          throw new NotSupportedException($"Type code '{type}' is not supported.");
      }
    }
  }
}
