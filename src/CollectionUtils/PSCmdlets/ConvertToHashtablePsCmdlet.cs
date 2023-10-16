using CollectionUtils.JoinCommandHandlers;
using CollectionUtils.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsData.ConvertTo, PSCmdletNouns.Hashtable)]
  [OutputType(typeof(Hashtable))]
  public sealed class ConvertToHashtablePSCmdlet : PSCmdlet, IDisposable
  {
    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 1,
      ValueFromPipeline = true)]
    public PSObject[] InputObject { get; set; } = default!;

    [Parameter(
      Mandatory = true,
      Position = 2)]
    public KeyParameter[] Key { get; set; } = default!;

    [Parameter(Position = 3)]
    public KeyComparerParameter? Comparer { get; set; }

    [Parameter(Position = 4)]
    public IEqualityComparer<string> DefaultStringComparer { get; set; } = StringComparer.OrdinalIgnoreCase;

    [Parameter()]
    public ConvertToHashtableKeyCollisionPreference KeyCollisionPreference { get; set; } = ConvertToHashtableKeyCollisionPreference.Error;

    #endregion

    private readonly CancellationTokenSource _CancellationTokenSource = new();

    private KeyField[] _KeyFields = default!;

    private IHashtableBuilder? _HashtableBuilder;

    private void ValidateComparers()
    {
      if (Comparer is null)
        return;

      foreach (var keyComparer in Comparer)
      {
        if (_KeyFields.FirstOrDefault(keyField => keyField.Property == keyComparer.Key) is null)
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

    protected override void BeginProcessing()
    {
      _KeyFields = Key.SelectMany(key => key).ToArray();

      ValidateComparers();

      _HashtableBuilder =
        KeyCollisionPreference == ConvertToHashtableKeyCollisionPreference.Group
        ? new ListOfPSObjectHashtableBuilder(
          _KeyFields,
          Comparer?.ToArray(),
          DefaultStringComparer)
        : new PSObjectHashtableBuilder(
          _KeyFields,
          Comparer?.ToArray(),
          DefaultStringComparer,
          KeyCollisionPreference.ToKeyCollisionPreference().SelectStrategy(new PowerShellWriter(this)));

      base.BeginProcessing();
    }

    protected override void ProcessRecord()
    {
      // TODO:
      //if (_ShouldStop)
      //  return;

      _HashtableBuilder!.AddObjects(InputObject);
      
      base.ProcessRecord();
    }

    protected override void EndProcessing()
    {
      WriteObject(_HashtableBuilder!.GetHashtable());

      base.EndProcessing();
    }

    protected override void StopProcessing()
    {
      _CancellationTokenSource.Cancel();

      base.StopProcessing();
    }

    public void Dispose()
    {
      _HashtableBuilder?.Dispose();
    }
  }
}
