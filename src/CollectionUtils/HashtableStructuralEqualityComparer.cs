using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CollectionUtils
{
  internal class HashtableStructuralEqualityComparer : EqualityComparer<Hashtable>
  {
    public HashtableStructuralEqualityComparer(
      string[] propertyNames,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer)
    {
      _DefaultStringComparer = defaultStringComparer;

      keyComparers ??= Array.Empty<KeyComparer>();

      foreach (var propertyName in propertyNames)
      {
        var comparer = keyComparers.FirstOrDefault(keyComparer => keyComparer.Property == propertyName)?.Comparer ?? EqualityComparer<object>.Default;

        _Comparers.Add((propertyName, comparer));
      }
    }

    private readonly List<(string PropertyName, IEqualityComparer Comparer)> _Comparers =
      new List<(string, IEqualityComparer)>();

    private readonly IEqualityComparer<string> _DefaultStringComparer;

    public override bool Equals(Hashtable? left, Hashtable? right)
    {
      if (left is null && right is null)
        return true;

      if (left is null ^ right is null)
        return false;

      if (left!.Keys.Count != right!.Keys.Count)
        return false;

      for (int i = 0; i < _Comparers.Count; i++)
      {
        (var propertyName, var comparer) = _Comparers[i];

        var leftValue = left[propertyName];
        var rightValue  = right[propertyName];

        // TODO: Not sure how efficient this is.
        if (leftValue is string leftString && rightValue is string rightString && comparer == EqualityComparer<object>.Default)
          if (!_DefaultStringComparer.Equals(leftString, rightString))
            return false;

        if (!comparer.Equals(left[propertyName], right[propertyName]))
          return false;
      }
      return true;
    }

    public override int GetHashCode([DisallowNull] Hashtable obj)
    {
      var hashCode = 0;

      for (int i = 0; i < _Comparers.Count; i++)
      {
        (var propertyName, var comparer) = _Comparers[i];

        hashCode ^= comparer.GetHashCode(obj[propertyName] ?? 0);
      }

      return hashCode;
    }
  }
}
