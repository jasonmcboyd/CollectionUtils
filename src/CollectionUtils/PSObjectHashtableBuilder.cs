using CollectionUtils.Exceptions;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils
{
  internal class PSObjectHashtableBuilder : HashtableBuilderBase<PSObject>
  {
    public PSObjectHashtableBuilder(
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer)
      : base(keyFields, keyComparers, defaultStringComparer)
    {
    }

    protected override void OnAddObject(PSObject obj)
    {
      var dict = GetInternalDictionary(obj);
      var key = KeySelector.GetKey(obj);

      if (dict.ContainsKey(key))
        throw new DuplicateKeyException(key, obj);

      dict.Add(key, obj);
    }

    protected override void OnDispose()
    {
    }

    protected override Hashtable OnGetHashtable()
    {
      var dict = GetInternalDictionary();

      // TODO: This is not correct because the hashtable will not have the same
      // comparer as the dictionary.
      if (dict is null)
        return new Hashtable();

      var result = new Hashtable(dict.Count, (IEqualityComparer)dict.Comparer);

      foreach ((var key, var value) in dict)
        result.Add(key, value);

      return result;
    }
  }
}
