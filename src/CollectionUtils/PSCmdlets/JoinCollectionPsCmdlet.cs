using CollectionUtils.Utilities;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsCommon.Join, PSCmdletNouns.Collection)]
  public class JoinCollectionPsCmdlet : PSCmdlet, IDisposable
  {
    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 1,
      ValueFromPipeline = true)]
    public PSObject[] LeftCollection { get; set; } = default!;

    [Parameter(
      Mandatory = true,
      Position = 2)]
    public PSObject[] RightCollection { get; set; } = default!;

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(Key),
      Position = 3)]
    public KeyField[]? Key { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 3)]
    public KeyField[] LeftKey { get; set; } = default!;

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 4)]
    public KeyField[] RightKey { get; set; } = default!;

    [Parameter(Position = 5)]
    public JoinType JoinType { get; set; } = JoinType.Outer;

    [Parameter(Position = 6)]
    public KeyComparer[]? Comparer { get; set; }

    [Parameter(Position = 7)]
    public IEqualityComparer<string> DefaultStringComparer { get; set; } = EqualityComparer<string>.Default;

    [Parameter(Position = 8)]
    public SwitchParameter Flatten { get; set; }

    #endregion Parameters

    private bool _ShouldStop = false;

    private ListOfPSObjectHashtableBuilder? _LeftHashtableBuilder;
    private ListOfPSObjectHashtableBuilder? _RightHashtableBuilder;

    private void ValidateKeyFields()
    {
      if (LeftKey.Length != RightKey.Length)
      {
        WriteError(
          new ErrorRecord(
            new PSArgumentException(
              "LeftKey and RightKey must have the same number of elements."),
              "KeyFieldLengthMismatch",
              ErrorCategory.InvalidArgument,
              null));

        _ShouldStop = true;
      }

      foreach (var leftKeyField in LeftKey)
      {
        var leftPropertyName = leftKeyField.Property;

        if (RightKey.FirstOrDefault(rightKeyField => rightKeyField.Property == leftPropertyName) is null)
        {
          WriteError(
            new ErrorRecord(
              new PSArgumentException(
                $"LeftKey contains a property name ({leftPropertyName}) that is not in RightKey."),
                "LeftKeyPropertyNameNotInRightKey",
                ErrorCategory.InvalidArgument,
                null));

          _ShouldStop = true;
        }
      }
    }

    private void ValidateComparers()
    {
      if (Comparer is null)
        return;

      foreach (var keyComparer in Comparer)
      {
        if (LeftKey.FirstOrDefault(leftKeyField => leftKeyField.Property == keyComparer.Property) is null)
        {
          WriteError(
            new ErrorRecord(
              new PSArgumentException(
                $"Comparer contains a property name ({keyComparer.Property}) that is not in the key."),
                "ComparerPropertyNameNotInKey",
                ErrorCategory.InvalidArgument,
                null));

          _ShouldStop = true;
        }
      }
    }

    protected override void BeginProcessing()
    {
      // This works because Key and LeftKey/RightKey are mutually exclusive.
      if (Key != null)
      {
        LeftKey = Key;
        RightKey = Key;
      }

      ValidateKeyFields();
      ValidateComparers();

      _RightHashtableBuilder = new ListOfPSObjectHashtableBuilder(RightKey, Comparer, DefaultStringComparer);
      _RightHashtableBuilder.AddObjects(RightCollection);

      _LeftHashtableBuilder = new ListOfPSObjectHashtableBuilder(LeftKey, Comparer, DefaultStringComparer);

      base.BeginProcessing();
    }

    protected override void ProcessRecord()
    {
      if (_ShouldStop)
        return;

      _LeftHashtableBuilder!.AddObjects(LeftCollection);

      base.ProcessRecord();
    }

    protected override void EndProcessing()
    {
      var joinedObjects = JoinLeftAndRight();

      foreach (var (left, right, key) in joinedObjects)
        if (Flatten)
          WriteFlattenedPSObjectList(left, right, key);
        else
          WritePSObjectList(left, right, key);

      base.EndProcessing();
    }

    protected override void StopProcessing()
    {
      _ShouldStop = true;

      base.StopProcessing();
    }

    private IEnumerable<(PSObject[]? Left, PSObject[]? Right, object Key)> JoinLeftAndRight()
    {
      var leftHashtable = _LeftHashtableBuilder!.GetHashtable();
      var rightHashtable = _RightHashtableBuilder!.GetHashtable();

      foreach (var key in leftHashtable.Keys)
      {
        var leftObjects = leftHashtable.Get<PSObject[]>(key);

        if (rightHashtable.TryRemove<PSObject[]>(key, out var rightObjects))
          yield return (leftObjects, rightObjects, key);
        else
          if (JoinType == JoinType.Left || JoinType == JoinType.Outer)
            yield return (leftObjects, null, key);
      }

      if (JoinType == JoinType.Right || JoinType == JoinType.Outer)
        foreach (var key in rightHashtable.Keys)
          yield return (null, rightHashtable.Get<PSObject[]>(key), key);

      base.EndProcessing();
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

    private void WritePSObjectList(PSObject[]? left, PSObject[]? right, object key) => WriteObject(CreatePSObject(left, right, key));

    private PSObject CreatePSObject(object? left, object? right, object key)
    {
      var psObject = new PSObject();

      psObject.Properties.Add(new PSNoteProperty("Key", key));
      psObject.Properties.Add(new PSNoteProperty("Left", left));
      psObject.Properties.Add(new PSNoteProperty("Right", right));

      return psObject;
    }

    public void Dispose()
    {
      _LeftHashtableBuilder?.Dispose();
      _RightHashtableBuilder?.Dispose();
    }
  }
}
