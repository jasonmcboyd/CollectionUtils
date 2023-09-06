using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace CollectionUtils.Utilities
{
  internal static class SharedStringBuilderPool
  {
    private static readonly ObjectPool<StringBuilder> _Pool = new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy());

    public static StringBuilder Get() => _Pool.Get();

    public static void Return(StringBuilder stringBuilder) => _Pool.Return(stringBuilder);

    public static ValueDisposable<StringBuilder> GetAsDisposable() => _Pool.GetAsDisposable();
  }
}
