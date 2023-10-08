using CollectionUtils.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CollectionUtils
{
  internal static class EqualityComparerFactory
  {
    public static IEqualityComparer Create(
      object obj,
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
      object obj,
      KeyField keyField,
      IEqualityComparer<string> defaultSringComparer) =>
      GetPropertyType(obj, keyField) == typeof(string)
      ? (IEqualityComparer)defaultSringComparer
      : EqualityComparer<object>.Default;

    private static Type GetPropertyType(object obj, KeyField keyField) =>
      PropertyGetter.GetProperty(obj, keyField)?.GetType() ?? throw new NullKeyException(obj);

    private static IEqualityComparer GetHashTableComparer(
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer)
    {
      var comparers =
        keyFields
        .GroupJoin(
          keyComparers ?? Enumerable.Empty<KeyComparer>(),
          keyField => keyField.Property,
          keyComparer => keyComparer.Key,
          (keyField, keyComparers) => keyComparers.FirstOrDefault() ?? new KeyComparer(keyField.Property),
          StringComparer.OrdinalIgnoreCase)
        .ToArray();

      return new HashtableStructuralEqualityComparer(
        comparers,
        defaultStringComparer);
    }
  }
}
