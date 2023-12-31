﻿using CollectionUtils.JoinCommandHandlers;
using CollectionUtils.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsCommon.Join, PSCmdletNouns.Collection)]
  [OutputType(typeof(PSObject[]))]
  public sealed class JoinCollectionPsCmdlet : PSCmdlet, IDisposable
  {
    #region Common Parameters

    [Parameter(
      Mandatory = true,
      Position = 1)]
    public IEnumerable Left { get; set; } = default!;

    [Parameter(
      Mandatory = true,
      Position = 2)]
    public IEnumerable Right { get; set; } = default!;

    #endregion Common Parameters

    #region Basic Join Parameters

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(CrossJoin),
      Position = 3)]
    public SwitchParameter CrossJoin { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(ZipJoin),
      Position = 3)]
    public SwitchParameter ZipJoin { get; set; }

    #endregion Basic Join Parameters

    #region Keyed Join Parameters

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(Key),
      Position = 3)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 3)]
    public SwitchParameter DisjunctJoin { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(InnerJoin) + "|" + nameof(Key),
      Position = 3)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(InnerJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 3)]
    public SwitchParameter InnerJoin { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(LeftJoin) + "|" + nameof(Key),
      Position = 3)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(LeftJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 3)]
    public SwitchParameter LeftJoin { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(OuterJoin) + "|" + nameof(Key),
      Position = 3)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(OuterJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 3)]
    public SwitchParameter OuterJoin { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(RightJoin) + "|" + nameof(Key),
      Position = 3)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(RightJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 3)]
    public SwitchParameter RightJoin { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(Key),
      Position = 4)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(InnerJoin) + "|" + nameof(Key),
      Position = 4)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(LeftJoin) + "|" + nameof(Key),
      Position = 4)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(OuterJoin) + "|" + nameof(Key),
      Position = 4)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(RightJoin) + "|" + nameof(Key),
      Position = 4)]
    public KeyParameter[]? Key { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 4)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(InnerJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 4)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(LeftJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 4)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(OuterJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 4)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(RightJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 4)]
    public KeyParameter[] LeftKey { get; set; } = default!;

    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 4)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(InnerJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 5)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(LeftJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 5)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(OuterJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 5)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = nameof(RightJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey),
      Position = 5)]
    public KeyParameter[] RightKey { get; set; } = default!;

    [Parameter(ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(InnerJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(LeftJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(OuterJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(RightJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(InnerJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(LeftJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(OuterJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(RightJoin) + "|" + nameof(Key))]
    public KeyComparerParameter? Comparer { get; set; }

    [Parameter(ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(InnerJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(LeftJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(OuterJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(RightJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(InnerJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(LeftJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(OuterJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(RightJoin) + "|" + nameof(Key))]
    public IEqualityComparer<string> DefaultStringComparer { get; set; } = StringComparer.OrdinalIgnoreCase;

    [Parameter(ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(InnerJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(LeftJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(OuterJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(RightJoin) + "|" + nameof(LeftKey) + "|" + nameof(RightKey))]
    [Parameter(ParameterSetName = nameof(DisjunctJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(InnerJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(LeftJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(OuterJoin) + "|" + nameof(Key))]
    [Parameter(ParameterSetName = nameof(RightJoin) + "|" + nameof(Key))]
    public JoinCollectionKeyCollisionPreference KeyCollisionPreference { get; set; } = JoinCollectionKeyCollisionPreference.Error;

    #endregion Keyed Join Parameters

    private IJoinCommandHandler? _CommandHandler = null;

    private readonly CancellationTokenSource _CancellationTokenSource = new();

    private KeyField[] _LeftKeyFields = default!;
    private KeyField[] _RightKeyFields = default!;

    private void ValidateKeyFields()
    {
      if (ZipJoin || CrossJoin)
        return;

      if (_LeftKeyFields.Length != _RightKeyFields.Length)
      {
        WriteError(
          new ErrorRecord(
            new PSArgumentException(
              "LeftKey and RightKey must have the same number of elements."),
              "KeyFieldLengthMismatch",
              ErrorCategory.InvalidArgument,
              null));

        _CancellationTokenSource.Cancel();
      }

      foreach (var leftKeyField in _LeftKeyFields)
      {
        var leftPropertyName = leftKeyField.Property;

        var rightKeyField =
          _RightKeyFields
          .FirstOrDefault(rightKeyField => rightKeyField.Property.Equals(leftPropertyName, StringComparison.OrdinalIgnoreCase));

        if (rightKeyField is null)
        {
          WriteError(
            new ErrorRecord(
              new PSArgumentException(
                $"LeftKey contains a property name ({leftPropertyName}) that is not in RightKey."),
                "LeftKeyPropertyNameNotInRightKey",
                ErrorCategory.InvalidArgument,
                null));

          _CancellationTokenSource.Cancel();
        }
      }
    }

    private void ValidateComparers()
    {
      if (Comparer is null)
        return;

      foreach (var keyComparer in Comparer)
      {
        if (_LeftKeyFields.All(leftKeyField => !leftKeyField.Property.Equals(keyComparer.Key, StringComparison.OrdinalIgnoreCase)))
        {
          WriteError(
            new ErrorRecord(
              new PSArgumentException(
                $"Comparer contains a property name ({keyComparer.Key}) that is not in the key."),
                "ComparerPropertyNameNotInKey",
                ErrorCategory.InvalidArgument,
                null));

          _CancellationTokenSource.Cancel();
        }
      }
    }

    private KeyedJoinType GetKeyedJoinType() =>
      DisjunctJoin
      ? KeyedJoinType.Disjunct
      : InnerJoin
      ? KeyedJoinType.Inner
      : LeftJoin
      ? KeyedJoinType.Left
      : OuterJoin
      ? KeyedJoinType.Outer
      : RightJoin
      ? KeyedJoinType.Right
      : throw new InvalidOperationException("No join type specified.");

    private IJoinCommandHandler GetCommandHandler()
    {
      var right = Right.Cast<object>().ToArray();

      if (ZipJoin)
        return new ZipJoinCommandHandler(right, new PowerShellWriter(this), _CancellationTokenSource.Token);

      if (CrossJoin)
        return new CrossJoinCommandHandler(right, new PowerShellWriter(this), _CancellationTokenSource.Token);

      return
        new KeyedJoinCommandHandler(
          right,
          _LeftKeyFields,
          _RightKeyFields,
          Comparer?.ToArray(),
          DefaultStringComparer,
          GetKeyedJoinType(),
          KeyCollisionPreference,
          new PowerShellWriter(this),
          _CancellationTokenSource.Token);
    }

    private void SetKeyFields()
    {
      if (Key is not null)
      {
        _LeftKeyFields = Key.SelectMany(key => key).ToArray();
        _RightKeyFields = _LeftKeyFields;
      }
      else if (LeftKey is not null && RightKey is not null)
      {
        _LeftKeyFields = LeftKey.SelectMany(key => key).ToArray();
        _RightKeyFields = RightKey.SelectMany(key => key).ToArray();
      }
    }

    protected override void BeginProcessing()
    {
      SetKeyFields();

      ValidateKeyFields();
      ValidateComparers();

      _CommandHandler = GetCommandHandler();

      base.BeginProcessing();
    }

    protected override void ProcessRecord()
    {
      var left =
        Left
        .Cast<object>()
        .Select(x => new PSObject(x))
        .ToArray();

      for (int i = 0; i < left.Length; i++)
        _CommandHandler!.Next(left[i]);

      base.ProcessRecord();
    }

    protected override void EndProcessing()
    {
      _CommandHandler!.WriteRemainingObjects();

      base.EndProcessing();
    }

    protected override void StopProcessing()
    {
      _CancellationTokenSource.Cancel();

      base.StopProcessing();
    }

    public void Dispose()
    {
      _CommandHandler?.Dispose();
      _CancellationTokenSource.Dispose();
    }
  }
}
