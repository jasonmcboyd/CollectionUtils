using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;

namespace CollectionUtils.Utilities
{
  // I copied this implementation from StringBuilderPooledObjectPolicy.
  internal class ListPooledObjectPolicy<T> : IPooledObjectPolicy<List<T>>
  {
    public ListPooledObjectPolicy()
    {
    }

    public ListPooledObjectPolicy(int intialCapacity)
    {
      InitialCapacity = intialCapacity;
    }

    public ListPooledObjectPolicy(int intialCapacity, int maxRetainedCapacity)
    {
      InitialCapacity = intialCapacity;
      MaximumRetainedCapacity = maxRetainedCapacity;
    }

    private int InitialCapacity { get; set; } = 100;

    private int MaximumRetainedCapacity { get; set; } = 4 * 1024;

    public List<T> Create() => new List<T>(InitialCapacity);

    public bool Return(List<T> obj)
    {
      if (obj.Capacity > MaximumRetainedCapacity)
      {
        // Too big. Discard this one.
        // Shouldn't need to clear the list here to allow the elements to be GC'd because the list should not be referenced by anything.
        return false;
      }

      // Clear to allow the elements to be GC'd.
      obj.Clear();

      return true;
    }
  }
}
