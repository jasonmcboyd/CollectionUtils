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
      if (!IsCompositeKey(keyFields) && keyComparers?.Length ==1)
        return keyComparers[0].Comparer;

      if (IsCompositeKey(keyFields))
        return GetHashTableComparer(keyFields, keyComparers, defaultStringComparer);

      return GetObjectComparer(obj, keyFields, defaultStringComparer);
    }

    private static bool IsCompositeKey(KeyField[] keyFields) => keyFields.Length > 1;

    private static IEqualityComparer GetObjectComparer(
      object obj,
      KeyField[] keyFields,
      IEqualityComparer<string> defaultSringComparer)
    {
      var keySelector = new KeySelector(keyFields);

      var key = keySelector.GetKey(obj);

      var type = key.GetType();

      if (type == typeof(string))
        return (IEqualityComparer)defaultSringComparer;

      return EqualityComparer<object>.Default;
    }

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
