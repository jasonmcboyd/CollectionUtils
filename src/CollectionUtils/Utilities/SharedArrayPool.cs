using System;
using System.Buffers;

namespace CollectionUtils.Utilities
{
  internal static class SharedArrayPool
  {
    public static ValueDisposable<T[]> RentAsDisposable<T>(this ArrayPool<T> arrayPool, int minimumLength) =>
      ValueDisposable.Create(arrayPool.Rent(minimumLength), value => arrayPool.Return(value));

    public static ValueDisposable<T[]> RentAsDisposable<T>(this ArrayPool<T> arrayPool, params T[] values) =>
      RentAsDisposable(arrayPool, values.AsSpan());

    public static ValueDisposable<T[]> RentAsDisposable<T>(
      this ArrayPool<T> arrayPool,
      Span<T> span) =>
      RentAsDisposable(arrayPool, span, value => value);

    public static ValueDisposable<TResult[]> RentAsDisposable<TSource, TResult>(
      this ArrayPool<TResult> arrayPool,
      Span<TSource> span,
      Func<TSource, TResult> map)
    {
      var result = arrayPool.Rent(span.Length);

      for (int i = 0; i < span.Length; i++)
        result[i] = map(span[i]);

      return ValueDisposable.Create(result, value => arrayPool.Return(value));
    }
  }
}
