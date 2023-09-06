using System.Buffers;

namespace CollectionUtils.Utilities
{
  internal static class SharedArrayPool
  {
    public static ValueDisposable<T[]> RentAsDisposable<T>(int minimumLength)
    {
      var pool = ArrayPool<T>.Shared;

      return ValueDisposable.Create(pool.Rent(minimumLength), value => pool.Return(value));
    }
  }
}
