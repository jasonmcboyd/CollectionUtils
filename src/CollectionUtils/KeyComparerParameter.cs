using System.Collections;
using System.Collections.Generic;

namespace CollectionUtils
{
  public class KeyComparerParameter : IEnumerable<KeyComparer>
  {
    public KeyComparerParameter(Hashtable hashtable)
    {
      KeyComparers =
        hashtable
        .Cast<DictionaryEntry>()
        .Select(entry => new KeyComparer((string)entry.Key, (IEqualityComparer)entry.Value))
        .ToDictionary(keyComparer => keyComparer.Property);
    }

    public KeyComparer this[string property]
    {
      get => KeyComparers[property];
    }

    private Dictionary<string, KeyComparer> KeyComparers { get; }

    public IEnumerator<KeyComparer> GetEnumerator() => KeyComparers.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
