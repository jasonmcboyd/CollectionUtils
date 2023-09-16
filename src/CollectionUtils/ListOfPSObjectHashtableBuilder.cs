using CollectionUtils.Utilities;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils
{
  internal class ListOfPSObjectHashtableBuilder : HashtableBuilderBase<ValueDisposable<List<PSObject>>, PSObject[]>
  {
    public ListOfPSObjectHashtableBuilder(
      PSObject[] objects,
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer)
      : base(objects, keyFields, keyComparers, defaultStringComparer, ResultSelector)
    {
    }

    public ListOfPSObjectHashtableBuilder(
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer)
      : base(keyFields, keyComparers, defaultStringComparer, ResultSelector)
    {
    }

    private static PSObject[] ResultSelector(ValueDisposable<List<PSObject>> list) => list.Value.ToArray();

    protected override void OnAddObjectRequested(PSObject psObject)
    {
      var key = KeySelector.GetKey(psObject);

      if (!TryGet(key, out var value))
      {
        value = SharedListPool<PSObject>.GetAsDisposable();
        TryAdd(psObject, _ => value);
      }

      var list = value.Value;

      list.Add(psObject);
    }
  }
}
