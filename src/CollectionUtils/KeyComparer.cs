using System.Collections;

namespace CollectionUtils
{
  public class KeyComparer
  {
    public KeyComparer(string property, IEqualityComparer comparer)
    {
      Property = property;
      Comparer = comparer;
    }

    public string Property { get; private set; }
    public IEqualityComparer Comparer { get; private set; }
  }
}
