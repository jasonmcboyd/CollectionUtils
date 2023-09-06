using System;

namespace CollectionUtils.Utilities
{
  internal struct ValueDisposable<T> : IDisposable
  {
    public ValueDisposable(T value, Action<T> onDispose)
    {
      Value = value;
      _OnDispose = onDispose;
    }

    public T Value { get; }

    private Action<T> _OnDispose;

    public void Dispose() => _OnDispose.Invoke(Value);
  }

  internal static class ValueDisposable
  {
    public static ValueDisposable<T> Create<T>(T value, Action<T> onDispose) => new ValueDisposable<T>(value, onDispose);
  }
}
