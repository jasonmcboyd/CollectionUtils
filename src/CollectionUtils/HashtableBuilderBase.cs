using CollectionUtils.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace CollectionUtils
{
  internal abstract class HashtableBuilderBase<TValue, TResult> : IHashtableBuilder
  {
    public HashtableBuilderBase(
      PSObject[] objects,
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer,
      Func<TValue, TResult> resultSelector)
      : this(keyFields, keyComparers, defaultStringComparer, resultSelector)
    {
      AddObjects(objects);
    }

    public HashtableBuilderBase(
      KeyField[] keyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer,
      Func<TValue, TResult> resultSelector)
    {
      _KeyFields = keyFields;
      _KeyComparers = keyComparers;
      _DefaultStringComparer = defaultStringComparer;
      _ResultSelector = resultSelector;

      KeySelector = new KeySelector(keyFields);
    }

    private readonly KeyField[] _KeyFields;
    private readonly KeyComparer[]? _KeyComparers;
    private readonly IEqualityComparer<string> _DefaultStringComparer;
    private readonly Func<TValue, TResult> _ResultSelector;

    protected KeySelector KeySelector { get; }

    private bool _IsDisposed;

    private IEqualityComparer? _EqualityComparer;
    private IEqualityComparer GetEqualityComparer(PSObject psObject)
    {
      if (_EqualityComparer is null)
        _EqualityComparer = EqualityComparerFactory.Create(psObject, _KeyFields, _KeyComparers, _DefaultStringComparer);

      return _EqualityComparer;
    }

    private Hashtable? _InternalDictionary;
    private Hashtable GetInternalDictionary(PSObject psObject)
    {
      if (_InternalDictionary is null)
        _InternalDictionary = new Hashtable(GetEqualityComparer(psObject));

      return _InternalDictionary;
    }

    public void AddObjects(PSObject[] objects)
    {
      for (int i = 0; i < objects.Length; i++)
        AddObject(objects[i]);
    }

    public void AddObject(PSObject psObject)
    {
      if (_IsDisposed)
        throw new ObjectDisposedException("HashtableBuilder");

      if (psObject.BaseObject is DataTable table)
        for (int i = 0; i < table.Rows.Count; i++)
          OnAddObjectRequested(PSObject.AsPSObject(table.Rows[i]));
      else
        OnAddObjectRequested(psObject);
    }

    protected bool TryGet(object key, [MaybeNullWhen(false)] out TValue value)
    {
      value = default;

      if (_InternalDictionary is null)
        return false;

      return _InternalDictionary.TryGet(key, out value);
    }

    protected abstract void OnAddObjectRequested(PSObject obj);

    protected bool TryAdd(PSObject psObject, Func<PSObject, TValue> valueSelector)
    {
      var dict = GetInternalDictionary(psObject);

      var key = KeySelector.GetKey(psObject);

      if (dict.ContainsKey(key))
        return false;

      dict.Add(key, valueSelector(psObject));
      return true;
    }

    public Hashtable GetHashtable()
    {
      if (_IsDisposed)
        throw new ObjectDisposedException("HashtableBuilder");

      var dict = _InternalDictionary;

      // TODO: This is not correct because the hashtable will not have the same
      // comparer as the dictionary.
      if (dict is null)
        return new Hashtable();

      var result = new Hashtable(dict.Count, _EqualityComparer);

      foreach ((var key, var value) in dict.Cast<DictionaryEntry>())
        result.Add(key, _ResultSelector((TValue)value));

      return result;
    }

    public void Dispose()
    {
      _IsDisposed = true;

      if (_InternalDictionary is null)
        return;

      foreach (var value in _InternalDictionary.Values)
        (value as IDisposable)?.Dispose();
    }
  }
}
