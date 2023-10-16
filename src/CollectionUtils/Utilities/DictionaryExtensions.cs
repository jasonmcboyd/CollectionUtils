using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CollectionUtils.Utilities
{
  internal static class DictionaryExtensions
  {
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
    {
      if (!dictionary.TryGetValue(key, out var result))
      {
        result = valueFactory();
        dictionary.Add(key, result);
      }

      return result;
    }

    public static bool TryGet<T>(this IDictionary dictionary, object key, [MaybeNullWhen(false)] out T value)
    {
      value = default!;

      if (!dictionary.Contains(key))
        return false;

      value = (T)dictionary[key]!;

      return true;
    }

    public static T Get<T>(this IDictionary dictionary, object key) => (T)dictionary[key]!;

    public static bool TryRemove<T>(this IDictionary dictionary, object key, [MaybeNullWhen(false)] out T value)
    {
      if (dictionary.TryGet(key, out value))
      {
        dictionary.Remove(key);
        return true;
      }

      return false;
    }
  }
}
