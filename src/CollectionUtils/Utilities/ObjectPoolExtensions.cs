using Microsoft.Extensions.ObjectPool;

namespace CollectionUtils.Utilities
{
  internal static class ObjectPoolExtensions
  {
    public static ValueDisposable<T> GetAsDisposable<T>(this ObjectPool<T> pool)
      where T : class =>
      ValueDisposable.Create(pool.Get(), pool.Return);
  }
}
