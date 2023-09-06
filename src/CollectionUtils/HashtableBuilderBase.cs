using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Management.Automation;

namespace CollectionUtils
{
  internal abstract class HashtableBuilderBase<T> : IDisposable
  {
    public HashtableBuilderBase(
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer)
    {
      _KeyFields = keyFields;
      _KeyComparers = keyComparers;
      _DefaultStringComparer = defaultStringComparer;
      KeySelector = new KeySelector(keyFields);
    }

    public readonly KeyField[] _KeyFields;
    public readonly KeyComparer[]? _KeyComparers;
    public readonly IEqualityComparer<string> _DefaultStringComparer;

    public int Count => _InternalDictionary?.Count ?? 0;

    protected KeySelector KeySelector { get; }

    private bool _IsDisposed;

    private IEqualityComparer? _EqualityComparer;
    private IEqualityComparer GetEqualityComparer(PSObject obj)
    {
      if (_EqualityComparer is null)
        _EqualityComparer = EqualityComparerFactory.Create(obj, _KeyFields, _KeyComparers, _DefaultStringComparer);

      return _EqualityComparer;
    }

    private Dictionary<object, T>? _InternalDictionary;
    protected Dictionary<object, T> GetInternalDictionary(PSObject obj)
    {
      if (_InternalDictionary is null)
        _InternalDictionary = new Dictionary<object, T>(GetEqualityComparer(obj) as IEqualityComparer<object>);

      return _InternalDictionary;
    }

    protected Dictionary<object, T>? GetInternalDictionary() => _InternalDictionary;

    public void AddObjects(PSObject[] inputObjects)
    {
      for (int i = 0; i < inputObjects.Length; i++)
        AddObject(inputObjects[i]);
    }

    private void AddObject(PSObject obj)
    {
      if (_IsDisposed)
        throw new ObjectDisposedException(nameof(HashtableBuilderBase<T>));

      if (obj.BaseObject is DataTable table)
        for (int i = 0; i < table.Rows.Count; i++)
          OnAddObject(PSObject.AsPSObject(table.Rows[i]));
      else
        OnAddObject(obj);
    }

    protected abstract void OnAddObject(PSObject obj);

    public Hashtable GetHashtable()
    {
      if (_IsDisposed)
        throw new ObjectDisposedException(nameof(HashtableBuilderBase<T>));

      return OnGetHashtable();
    }

    protected abstract Hashtable OnGetHashtable();

    public void Dispose()
    {
      _IsDisposed = true;

      OnDispose();
    }
    
    protected abstract void OnDispose();
  }
}
