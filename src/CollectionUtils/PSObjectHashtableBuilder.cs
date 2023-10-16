using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils
{
  internal class PSObjectHashtableBuilder : HashtableBuilderBase<object, PSObject>
  {
    public PSObjectHashtableBuilder(
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer,
      KeyCollisionStrategy keyCollisionStrategy)
      : base(keyFields, keyComparers, defaultStringComparer, ResultSelector)
    {
      _KeyCollisionStrategy = keyCollisionStrategy;
    }

    private static PSObject ResultSelector(object obj) => new(obj);

    private readonly KeyCollisionStrategy _KeyCollisionStrategy;

    protected override void OnAddObjectRequested(object obj)
    {
      var key = KeySelector.GetKey(obj);

      if (!TryAdd(obj, x => x))
        _KeyCollisionStrategy(key, obj);
    }
  }
}
