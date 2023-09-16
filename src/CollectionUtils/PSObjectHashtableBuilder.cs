using CollectionUtils.Exceptions;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils
{
  internal class PSObjectHashtableBuilder : HashtableBuilderBase<PSObject, PSObject>
  {
    public PSObjectHashtableBuilder(
      PSObject[] objects,
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

    private static PSObject ResultSelector(PSObject psObject) => psObject;

    protected override void OnAddObjectRequested(PSObject psObject)
    {
      var key = KeySelector.GetKey(psObject);

      if (!TryAdd(psObject, obj => obj))
        throw new DuplicateKeyException(key, psObject);
    }
  }
}
