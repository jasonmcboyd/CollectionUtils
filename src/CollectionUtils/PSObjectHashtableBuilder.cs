using CollectionUtils.Exceptions;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils
{
  internal class PSObjectHashtableBuilder : HashtableBuilderBase<object, PSObject>
  {
    public PSObjectHashtableBuilder(
      object[] objects,
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer)
      : base(objects, keyFields, keyComparers, defaultStringComparer, ResultSelector)
    {
    }

    public PSObjectHashtableBuilder(
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer)
      : base(keyFields, keyComparers, defaultStringComparer, ResultSelector)
    {
    }

    private static PSObject ResultSelector(object obj) => new PSObject(obj);

    protected override void OnAddObjectRequested(object obj)
    {
      var key = KeySelector.GetKey(obj);

      if (!TryAdd(obj, x => x))
        throw new DuplicateKeyException(key, obj);
    }
  }
}
