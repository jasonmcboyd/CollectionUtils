using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsData.ConvertTo, PSCmdletNouns.Hashtable)]
  [OutputType(typeof(Hashtable))]
  public class ConvertToHashtablePsCmdlet : PSCmdlet, IDisposable
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
    public KeyField[] Key { get; set; } = default!;

    [Parameter(Position = 3)]
    public KeyComparer[]? Comparer { get; set; }

    [Parameter(Position = 4)]
    public IEqualityComparer<string> DefaultStringComparer { get; set; } = StringComparer.OrdinalIgnoreCase;

    [Parameter()]
    public SwitchParameter AsLookup { get; set; }

    #endregion

    private bool _ShouldStop = false;

    private PSObjectHashtableBuilder? _PSObjectHashtableBuilder;
    private ListOfPSObjectHashtableBuilder? _ListOfPSObjectHashtableBuilder;

    private void ValidateComparers()
    {
      if (Comparer is null)
        return;

      foreach (var keyComparer in Comparer)
      {
        if (Key.FirstOrDefault(keyField => keyField.Property == keyComparer.Property) is null)
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
      ValidateComparers();

      if (AsLookup)
        _ListOfPSObjectHashtableBuilder = new ListOfPSObjectHashtableBuilder(Key, Comparer, DefaultStringComparer);
      else
        _PSObjectHashtableBuilder = new PSObjectHashtableBuilder(Key, Comparer, DefaultStringComparer);

      base.BeginProcessing();
    }

    protected override void ProcessRecord()
    {
      if (_ShouldStop)
        return;

      if (AsLookup)
        _ListOfPSObjectHashtableBuilder!.AddObjects(InputObject);
      else
        _PSObjectHashtableBuilder!.AddObjects(InputObject);

      base.ProcessRecord();
    }

    protected override void EndProcessing()
    {
      if (AsLookup)
        WriteObject(_ListOfPSObjectHashtableBuilder!.GetHashtable());
      else
        WriteObject(_PSObjectHashtableBuilder!.GetHashtable());

      base.EndProcessing();
    }

    protected override void StopProcessing()
    {
      _ShouldStop = true;

      base.StopProcessing();
    }

    public void Dispose()
    {
      _ListOfPSObjectHashtableBuilder?.Dispose();
      _PSObjectHashtableBuilder?.Dispose();
    }
  }
}
