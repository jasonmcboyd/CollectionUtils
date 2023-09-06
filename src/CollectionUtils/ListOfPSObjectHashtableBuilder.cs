using CollectionUtils.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils
{
  internal class ListOfPSObjectHashtableBuilder : HashtableBuilderBase<List<PSObject>>
  {
    public ListOfPSObjectHashtableBuilder(
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

      if (!dict.TryGetValue(key, out var list))
      {
        list = SharedListPool<PSObject>.Get();
        dict.Add(key, list);
      }

      list.Add(obj);
    }

    protected override void OnDispose()
    {
      var dict = GetInternalDictionary();

      if (dict is null)
        return;

      foreach (var list in dict.Values)
        SharedListPool<PSObject>.Return(list);
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
        result.Add(key, value.ToArray());

      return result;
    }
  }
}
