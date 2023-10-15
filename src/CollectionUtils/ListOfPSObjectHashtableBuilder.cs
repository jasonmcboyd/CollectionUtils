using CollectionUtils.Utilities;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils
{
  internal class ListOfPSObjectHashtableBuilder : HashtableBuilderBase<ValueDisposable<List<object>>, PSObject[]>
  {
    public ListOfPSObjectHashtableBuilder(
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer)
      : base(keyFields, keyComparers, defaultStringComparer, ResultSelector)
    {
    }

    private static PSObject[] ResultSelector(ValueDisposable<List<object>> list) => list.Value.Select(obj => new PSObject(obj)).ToArray();

    protected override void OnAddObjectRequested(object obj)
    {
      var key = KeySelector.GetKey(obj);

      if (!TryGet(key, out var value))
      {
        value = SharedListPool<object>.GetAsDisposable();
        TryAdd(obj, _ => value);
      }

      var list = value.Value;

      list.Add(obj);
    }
  }
}
