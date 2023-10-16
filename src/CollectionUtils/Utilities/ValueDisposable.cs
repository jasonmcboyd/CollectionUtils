using System;

namespace CollectionUtils.Utilities
{
  internal readonly struct ValueDisposable<T> : IDisposable
  {
    public ValueDisposable(T value, Action<T> onDispose) : this(value)
    {
      _OnDispose = onDispose;
    }

    public ValueDisposable(T value)
    {
      Value = value;
    }

    public T Value { get; }

    private readonly Action<T>? _OnDispose = null;

    public void Dispose() => _OnDispose?.Invoke(Value);
  }

  internal static class ValueDisposable
  {
    public static ValueDisposable<T> Create<T>(T value, Action<T> onDispose) => new(value, onDispose);
    public static ValueDisposable<T> Create<T>(T value) => new(value);
  }
}
