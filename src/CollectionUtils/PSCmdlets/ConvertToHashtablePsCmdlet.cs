using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsData.ConvertTo, PSCmdletNouns.Hashtable)]
  [OutputType(typeof(Hashtable))]
  public class ConvertToHashtablePSCmdlet : PSCmdlet, IDisposable
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
    public SwitchParameter AsLookup { get; set; }

    #endregion

    private CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

    private KeyField[] _KeyFields = default!;

    private IHashtableBuilder? _HashtableBuilder;

    private void ValidateComparers()
    {
      if (Comparer is null)
        return;

      foreach (var keyComparer in Comparer)
      {
        if (_KeyFields.FirstOrDefault(keyField => keyField.Property == keyComparer.Property) is null)
        {
          WriteError(
            new ErrorRecord(
              new PSArgumentException(
                $"Comparer contains a property name ({keyComparer.Property}) that is not in the key."),
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

      if (AsLookup)
        _HashtableBuilder = new ListOfPSObjectHashtableBuilder(_KeyFields, Comparer?.ToArray(), DefaultStringComparer);
      else
        _HashtableBuilder = new PSObjectHashtableBuilder(_KeyFields, Comparer?.ToArray(), DefaultStringComparer);

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
