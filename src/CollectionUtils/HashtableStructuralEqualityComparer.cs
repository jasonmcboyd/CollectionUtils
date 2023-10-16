using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CollectionUtils
{
  internal class HashtableStructuralEqualityComparer : EqualityComparer<Hashtable>
  {
    public HashtableStructuralEqualityComparer(params string[] keysToCompare)
      : this(keysToCompare.Select(key => new KeyComparer(key)).ToArray(), null)
    {
    }

    public HashtableStructuralEqualityComparer(params KeyComparer[] keyComparers)
      : this(keyComparers, null)
    {
    }

    public HashtableStructuralEqualityComparer(
      KeyComparer[] keyComparers,
      IEqualityComparer<string>? defaultStringComparer)
    {
      _DefaultStringComparer = defaultStringComparer ?? StringComparer.OrdinalIgnoreCase;

      _KeyComparers = keyComparers;
    }

    private readonly KeyComparer[] _KeyComparers;

    private readonly IEqualityComparer<string> _DefaultStringComparer;

    public override bool Equals(Hashtable? left, Hashtable? right)
    {
      if (left is null && right is null)
        return true;

      if (left is null ^ right is null)
        return false;

      for (int i = 0; i < _KeyComparers.Length; i++)
      {
        (var key, var comparer) = _KeyComparers[i];

        var leftValue = left![key];
        var rightValue  = right![key];

        // TODO: Not sure how efficient this is.
        if (leftValue is string leftString && rightValue is string rightString && comparer == EqualityComparer<object>.Default)
        {
          if (!_DefaultStringComparer.Equals(leftString, rightString))
            return false;
          else
            continue;
        }
        
        if (!comparer.Equals(leftValue, rightValue))
          return false;
      }

      return true;
    }

    public override int GetHashCode([DisallowNull] Hashtable obj)
    {
      var hashCode = 0;

      for (int i = 0; i < _KeyComparers.Length; i++)
      {
        (var key, var comparer) = _KeyComparers[i];

        var value = obj[key];

        if (value is string && comparer == EqualityComparer<object>.Default)
          hashCode ^= _DefaultStringComparer.GetHashCode((string)value!);
        else if (value is null)
          hashCode ^= 0;
        else
          hashCode ^= comparer.GetHashCode(value);
      }

      return hashCode;
    }
  }
}
