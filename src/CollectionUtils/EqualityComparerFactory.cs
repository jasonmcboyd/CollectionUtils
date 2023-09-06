using CollectionUtils.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils
{
  internal static class EqualityComparerFactory
  {
    public static IEqualityComparer Create(
      PSObject obj,
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer)
    {
      if (IsCompositeKey(keyFields))
        return GetHashTableComparer(keyFields, keyComparers, defaultStringComparer);
      else if (keyComparers?.Length == 1)
        return keyComparers[0].Comparer;
      else
        return GetObjectComparer(obj, keyFields[0], defaultStringComparer);
    }

    private static bool IsCompositeKey(KeyField[] keyFields) => keyFields.Length > 1;

    private static IEqualityComparer GetObjectComparer(
      PSObject obj,
      KeyField keyField,
      IEqualityComparer<string> defaultSringComparer) =>
      GetPropertyType(obj, keyField) == typeof(string)
      ? (IEqualityComparer)defaultSringComparer
      : EqualityComparer<object>.Default;

    private static Type GetPropertyType(PSObject obj, KeyField keyField) =>
      PropertyGetter.GetProperty(obj, keyField)?.GetType() ?? throw new NullKeyException(obj);

    private static IEqualityComparer GetHashTableComparer(
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer) =>
      new HashtableStructuralEqualityComparer(
        keyFields.Select(key => key.Property).ToArray(),
        keyComparers,
        defaultStringComparer);
  }
}
