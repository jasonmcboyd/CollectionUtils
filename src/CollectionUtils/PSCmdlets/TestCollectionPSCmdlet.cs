using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsDiagnostic.Test, PSCmdletNouns.Collection)]
  [OutputType(typeof(bool))]
  public class TestCollectionPSCmdlet : PSCmdlet
  {
    #region Parameters

    [Parameter(
      Mandatory = true,
      ParameterSetName = $"{nameof(InputObject)}|{nameof(PredicateScript)}",
      Position = 0,
      ValueFromPipeline = true)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = $"{nameof(InputObject)}|{nameof(PredicateScript)}|{nameof(All)}",
      Position = 0,
      ValueFromPipeline = true)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = $"{nameof(InputObject)}|{nameof(PredicateScript)}|{nameof(Any)}",
      Position = 0,
      ValueFromPipeline = true)]
    public PSObject[]? InputObject { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = $"{nameof(InputObject)}|{nameof(PredicateScript)}",
      Position = 1)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = $"{nameof(InputObject)}|{nameof(PredicateScript)}|{nameof(All)}",
      Position = 1)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = $"{nameof(InputObject)}|{nameof(PredicateScript)}|{nameof(Any)}",
      Position = 1)]
    public ScriptBlock? PredicateScript { get; set; }

    [Parameter(Position = 2, ParameterSetName = $"{nameof(InputObject)}|{nameof(PredicateScript)}|{nameof(All)}")]
    public SwitchParameter All { get; set; }

    [Parameter(Position = 2, ParameterSetName = $"{nameof(InputObject)}|{nameof(PredicateScript)}|{nameof(Any)}")]
    public SwitchParameter Any { get; set; }

    #endregion Parameters

    private enum Quantifier
    {
      All,
      Any
    }

    private Quantifier SelectedQuantifier => All || !Any ? Quantifier.All : Quantifier.Any;

    private bool _ShouldStop = false;
    private bool? _Result = null;

    protected override void BeginProcessing()
    {
      WriteDebug("Quantifier: " + SelectedQuantifier.ToString());
      base.BeginProcessing();
    }

    protected override void ProcessRecord()
    {
      if (_ShouldStop)
        return;

      foreach (var obj in InputObject!)
      {
        // This reversal of true / false is confusing.
        var scriptBlockVariables = new List<PSVariable>
        {
            new PSVariable("_", obj)
        };

        var resultsFromScriptBlock = PredicateScript?.InvokeWithContext(
            functionsToDefine: null,
            variablesToDefine: scriptBlockVariables,
            args: null
        );

        if (resultsFromScriptBlock!.Count != 1 || (resultsFromScriptBlock.Count >= 1 && resultsFromScriptBlock[0]?.BaseObject is not bool))
          WriteWarning("The script block did not return a single boolean value.");

        var scriptBlockReturnsFalse = resultsFromScriptBlock!.Select(x => x?.BaseObject).OfType<bool>().Any(x => !x);

        WriteDebug($"{obj}: " + !scriptBlockReturnsFalse);

        switch ((SelectedQuantifier!, scriptBlockReturnsFalse))
        {
          case (Quantifier.All, true):
          case (Quantifier.Any, false):
            _Result = !scriptBlockReturnsFalse;
            StopProcessing();
            return;
          default:
            // Do nothing.
            break;
        }
      }

      base.ProcessRecord();
    }

    protected override void EndProcessing()
    {
      WriteObject(_Result != null ? _Result.Value : SelectedQuantifier == Quantifier.All);

      base.EndProcessing();
    }

    protected override void StopProcessing()
    {
      _ShouldStop = true;
    }
  }
}
