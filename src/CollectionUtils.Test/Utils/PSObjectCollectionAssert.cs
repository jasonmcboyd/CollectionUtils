using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils.Test.Utils
{
  internal static class PSObjectCollectionAssert
  {
    public static void AreEqual(IEnumerable<PSObject> psObjects, IEnumerable values)
    {
      if (psObjects is null && values is null)
        return;

      if (psObjects is null ^ values is null)
        throw new AssertFailedException("PSObjectCollectionAssert.AreEqual");

      using var objectsEnumerator = psObjects!.GetEnumerator();
      var valuesEnumerator = values!.GetEnumerator();

      while (true)
      {
        var hasNextObject = objectsEnumerator.MoveNext();
        var hasNextValue = valuesEnumerator.MoveNext();

        if (!hasNextObject && !hasNextValue)
          return;

        if (hasNextObject ^ hasNextValue)
          throw new AssertFailedException("PSObjectCollectionAssert.AreEqual");

        if (!object.Equals(objectsEnumerator.Current.BaseObject, valuesEnumerator.Current))
          throw new AssertFailedException("PSObjectCollectionAssert.AreEqual");
      }
    }
  }
}
