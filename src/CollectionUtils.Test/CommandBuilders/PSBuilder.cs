using CollectionUtils.Test.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils.Test.CommandBuilders
{
  internal static class PSBuilder
  {
    public static ConvertFromDataTableCommandBuilder ConvertFromDataTable() => new ConvertFromDataTableCommandBuilder();

    public static ConvertToHashtableCommandBuilder ConvertToHashTable() => new ConvertToHashtableCommandBuilder();

    public static JoinCollectionCommandBuilder JoinCollection() => new JoinCollectionCommandBuilder();

    public static string ToPSCustomObjectString(this IEnumerable<(string Key, object Value)> keyValuePairs) =>
      $"[pscustomobject]{ToPSHashtableString(keyValuePairs)}";

    private static string ObjectToPSString(object? value)
    {
      if (value is null)
        return "$null";

      if (value is string)
        return $"'{value}'";

      return value.ToString()!;
    }

    public static string ToPSHashtableString(this IEnumerable<(string Key, object Value)> keyValuePairs)
    {
      var keyValuePairsString =
        keyValuePairs
        .Select(keyValuePair => $"'{keyValuePair.Key}' = {ObjectToPSString(keyValuePair.Value)}").JoinStrings("; ");

      return $"@{{ {keyValuePairsString} }}";
    }

    public static string ToPSArrayString(this IEnumerable<string> values) =>
      $"@({values.JoinStrings(", ")})";

    public static string ToPSArrayOfPSCustomObjectsString(this IEnumerable<IEnumerable<(string Key, object Value)>> values) =>
      values.Select(ToPSCustomObjectString).ToPSArrayString();

    public static string ToPSArrayOfPSHashtableString(this IEnumerable<IEnumerable<(string Key, object Value)>> values) =>
      values.Select(ToPSHashtableString).ToPSArrayString();

    public static string KeyParameter(string property, string expression) =>
      $"@{{ {property} = {{ {expression} }} }}";

    public static string KeyField(string property, string expression) =>
      $"[{typeof(KeyField).FullName}]::new('{property}', {{ {expression} }})";
  }
}
