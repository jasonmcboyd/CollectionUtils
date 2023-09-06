using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;

namespace CollectionUtils.Utilities
{
  internal static class SharedListPool<T>
  {
    private static readonly ObjectPool<List<T>> _Pool = new DefaultObjectPool<List<T>>(new ListPooledObjectPolicy<T>(10, 1024));

    public static List<T> Get() => _Pool.Get();

    public static void Return(List<T> list) => _Pool.Return(list);

    public static ValueDisposable<List<T>> GetAsDisposable() => _Pool.GetAsDisposable();
  }
}
