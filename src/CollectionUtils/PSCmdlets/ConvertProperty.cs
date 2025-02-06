using System;
using System.Management.Automation;

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
    public TypeCode Type { get; set; } = default!;

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
          !property.Name.Equals(Property, StringComparison.OrdinalIgnoreCase)
          ? property.Value
          : property.Value is string stringValue
          ? TypeConverter.Parse(stringValue, Type)
          : TypeConverter.Convert(property.Value, Type);

        psObject.Properties.Add(new PSNoteProperty(property.Name, newValue));
      }

      return psObject;
    }

    protected override void StopProcessing()
    {
      _ShouldStop = true;

      base.StopProcessing();
    }
  }
}
