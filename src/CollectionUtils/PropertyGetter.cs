using CollectionUtils.Exceptions;
using CollectionUtils.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Management.Automation;
using System.Reflection;

namespace CollectionUtils
{
  public static class PropertyGetter
  {
    public static object? GetProperty(
       PSObject psObject,
       KeyField keyField)
    {
      if (keyField.Expression is not null)
        return GetObjectPropertyWithScriptblock(psObject, keyField.Expression);

      if (psObject.BaseObject is null || psObject.BaseObject is PSCustomObject)
        return GetPsObjectProperty(psObject, keyField.Property);

      if (psObject.BaseObject is IDictionary dictionary)
        return GetDictionaryProperty(dictionary, keyField.Property);

      if (psObject.BaseObject is DataRow dataRow)
        return GetDataRowProperty(dataRow, keyField.Property);

      return GetObjectProperty(psObject.BaseObject, keyField.Property);
    }

    private static object? GetObjectPropertyWithScriptblock(
      PSObject psObject,
      ScriptBlock scriptBlock)
    {
      using var disposable = SharedListPool<PSVariable>.GetAsDisposable();

      var scriptBlockVariables = disposable.Value;

      var variable = new PSVariable("_", psObject);

      scriptBlockVariables.Add(variable);

      var resultsFromScriptBlock =
        scriptBlock
        .InvokeWithContext(
          functionsToDefine: null,
          variablesToDefine: scriptBlockVariables,
          args: null);

      return resultsFromScriptBlock[0];
    }

    private static object? GetPsObjectProperty(
      PSObject psObject,
      string propertyName)
    {
      // TODO: How do I check if a property exists without instantiating an Enumerable?
      // I need a way to distinguish between a property that doesn't exist and a property
      // with a value that is null.
      var propertyInfo = psObject.Properties[propertyName];

      return propertyInfo.Value;
    }

    private static object? GetDictionaryProperty(
      IDictionary dictionary,
      string propertyName) => dictionary[propertyName];

    private static object? GetDataRowProperty(DataRow dataRow, string propertyName) => dataRow[propertyName];

    private static readonly Dictionary<(string, string), PropertyInfo> _PropertyInfos = new Dictionary<(string, string), PropertyInfo>(new TupleComparer());

    private static object? GetObjectProperty(
      object obj,
      string propertyName)
    {
      var type = obj.GetType();

      var assemblyQualifiedName = type.AssemblyQualifiedName;

      if (assemblyQualifiedName is null)
        throw new PropertyResolutionException(obj, propertyName);

      var propertyInfo =
        _PropertyInfos
        .GetOrAdd(
          (type.AssemblyQualifiedName!, propertyName),
          () => type.GetProperties().FirstOrDefault(property => property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)) ?? throw new PropertyResolutionException(obj, propertyName));

      return propertyInfo.GetValue(obj);
    }

    class TupleComparer : IEqualityComparer<(string, string)>
    {
      public bool Equals((string, string) left, (string, string) right) =>
        StringComparer.OrdinalIgnoreCase.Equals(left.Item1, right.Item1)
        && StringComparer.OrdinalIgnoreCase.Equals(left.Item2, right.Item2);

      public int GetHashCode((string, string) tuple) =>
        HashCode
        .Combine(
          StringComparer.OrdinalIgnoreCase.GetHashCode(tuple.Item1),
          StringComparer.OrdinalIgnoreCase.GetHashCode(tuple.Item2));
    }
  }
}
