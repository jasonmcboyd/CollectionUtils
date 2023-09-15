using CollectionUtils.Utilities;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.JoinCommandHandlers
{
  internal class KeyedJoinCommandHandler : JoinCommandHandlerBase
  {
    public KeyedJoinCommandHandler(
      PSObject[] rightCollection,
      KeyField[] leftKeyFields,
      KeyField[] rightKeyFields,
      KeyComparer[]? keyComparers,
      IEqualityComparer<string> defaultStringComparer,
      KeyedJoinType keyedJoinType,
      GroupJoinStrategy groupJoinStrategy,
      Action<object> objectWriter,
      Action<ErrorRecord> errorWriter,
      CancellationToken cancellationToken)
      : base(rightCollection, objectWriter, errorWriter, cancellationToken)
    {
      _KeyedJoinType = keyedJoinType;
      _GroupJoinStrategy = groupJoinStrategy;

      if (groupJoinStrategy == GroupJoinStrategy.Error)
      {
        _RightHashtableBuilder = new PSObjectHashtableBuilder(rightCollection, rightKeyFields, keyComparers, defaultStringComparer);
        _LeftHashtableBuilder = new PSObjectHashtableBuilder(leftKeyFields, keyComparers, defaultStringComparer);
      }
      else
      {
        _RightHashtableBuilder = new ListOfPSObjectHashtableBuilder(rightCollection, rightKeyFields, keyComparers, defaultStringComparer);
        _LeftHashtableBuilder = new ListOfPSObjectHashtableBuilder(leftKeyFields, keyComparers, defaultStringComparer);
      }
    }

    private readonly KeyedJoinType _KeyedJoinType;
    private readonly GroupJoinStrategy _GroupJoinStrategy;

    private IHashtableBuilder _LeftHashtableBuilder;
    private IHashtableBuilder _RightHashtableBuilder;
    public override void Next(PSObject left)
    {
      if (CancellationToken.IsCancellationRequested)
        throw new OperationCanceledException();

      _LeftHashtableBuilder.AddObject(left);
    }

    override public void WriteRemainingObjects()
    {
      if (_GroupJoinStrategy == GroupJoinStrategy.Group)
        foreach (var (left, right, key) in JoinLeftAndRight<PSObject[]>())
          WritePSObject(left, right, key);

      else if (_GroupJoinStrategy == GroupJoinStrategy.Flatten)
        foreach (var (left, right, key) in JoinLeftAndRight<PSObject[]>())
          WriteFlattenedPSObjectList(left, right, key);

      else
        foreach (var (left, right, key) in JoinLeftAndRight<PSObject>())
          WritePSObject(left, right, key);
    }

    private IEnumerable<(T? Left, T? Right, object Key)> JoinLeftAndRight<T>()
      where T : class
    {
      var leftHashtable = _LeftHashtableBuilder!.GetHashtable();
      var rightHashtable = _RightHashtableBuilder!.GetHashtable();

      foreach (var key in leftHashtable.Keys)
      {
        var leftObjects = leftHashtable.Get<T>(key);

        if (rightHashtable.TryRemove<T>(key, out var rightObjects))
          yield return (leftObjects, rightObjects, key);
        else
          if (_KeyedJoinType == KeyedJoinType.Left || _KeyedJoinType == KeyedJoinType.Outer)
            yield return (leftObjects, null, key);
      }

      if (_KeyedJoinType == KeyedJoinType.Right || _KeyedJoinType == KeyedJoinType.Outer)
        foreach (var key in rightHashtable.Keys)
          yield return (null, rightHashtable.Get<T>(key), key);
    }

    private void WriteFlattenedPSObjectList(PSObject[]? left, PSObject[]? right, object key)
    {
      if (left?.Length > 0 && right?.Length > 0)
        for (var l = 0; l < left.Length; l++)
          for (var r = 0; r < right.Length; r++)
            WriteObject(CreatePSObject(left[l], right[r], key));

      else if (left?.Length > 0)
        for (var l = 0; l < left.Length; l++)
          WriteObject(CreatePSObject(left[l], null, key));

      else
        for (var r = 0; r < right!.Length; r++)
          WriteObject(CreatePSObject(null, right[r], key));
    }

    private void WritePSObject(object? left, object? right, object key) => WriteObject(CreatePSObject(left, right, key));

    private PSObject CreatePSObject(object? left, object? right, object key)
    {
      var psObject = CreatePSObject(left, right);
      
      psObject.Properties.Add(new PSNoteProperty("Key", key));

      return psObject;
    }

    public override void Dispose()
    {
      _LeftHashtableBuilder?.Dispose();
      _RightHashtableBuilder?.Dispose();
    }
  }
}
