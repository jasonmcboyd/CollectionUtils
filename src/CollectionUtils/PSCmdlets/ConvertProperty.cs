using System;
using System.IO;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.PSCmdlets
{
  [Cmdlet(VerbsData.Convert, PSCmdletNouns.Property)]
  [OutputType(typeof(PSObject[]))]
  public sealed class ConvertProperty : PSCmdlet
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
    public string Property { get; set; } = default!;

    [Parameter(
      Mandatory = true,
      Position = 3)]
    public ValueType Type { get; set; } = default!;

    #endregion

    private bool _ShouldStop;

    protected override void ProcessRecord()
    {
      foreach (var obj in InputObject)
      {
        if (_ShouldStop)
          break;

        WriteObject(BuildNewObject(obj));
      }

      base.ProcessRecord();
    }

    private PSObject BuildNewObject(PSObject obj)
    {
      if (obj.Properties[Property] == null)
        return obj;

      var psObject = new PSObject();

      foreach (var property in obj.Properties)
      {
        var newValue =
          property.Name.Equals(Property, StringComparison.OrdinalIgnoreCase)
          ? GetPropertyValue(property.Value)
          : property.Value;

        psObject.Properties.Add(new PSNoteProperty(property.Name, newValue));
      }

      return psObject;
    }

    private object? GetPropertyValue(object? value)
    {
      if (value == null)
        return null;

      return Type switch
      {
        ValueType.Boolean => Convert.ChangeType(value, TypeCode.Boolean),
        ValueType.DateTime => Convert.ChangeType(value, TypeCode.DateTime),
        ValueType.Decimal => Convert.ChangeType(value, TypeCode.Decimal),
        ValueType.Integer => Convert.ChangeType(value, TypeCode.Int32),
        ValueType.String => value.ToString(),
        _ => throw new NotImplementedException($"Conversion has not been implemented for '{Type}'.")
      };
    }

    protected override void StopProcessing()
    {
      _ShouldStop = true;

      base.StopProcessing();
    }


  }
}
