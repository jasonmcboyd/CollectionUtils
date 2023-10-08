using System.Collections;
using System.Collections.Generic;

namespace CollectionUtils
{
  public class KeyComparer
  {
    public KeyComparer(string key) : this(key, EqualityComparer<object>.Default)
    {
    }

    public KeyComparer(string key, IEqualityComparer comparer)
    {
      Key = key;
      Comparer = comparer;
    }

    public string Key { get; private set; }
    public IEqualityComparer Comparer { get; private set; }

    public void Deconstruct(out string key, out IEqualityComparer comparer)
    {
      key = Key;
      comparer = Comparer;
    }
  }
}
