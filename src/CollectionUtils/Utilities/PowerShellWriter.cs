using System.Management.Automation;

namespace CollectionUtils.Utilities
{
  public class PowerShellWriter
  {
    public PowerShellWriter(Cmdlet cmdlet)
    {
      Cmdlet = cmdlet;
    }

    private Cmdlet Cmdlet { get; }

    public void WriteObject(object obj) => Cmdlet.WriteObject(obj);
    public void WriteError(ErrorRecord errorRecord) => Cmdlet.WriteError(errorRecord);
    public void WriteVerbose(string message) => Cmdlet.WriteVerbose(message);
    public void WriteDebug(string message) => Cmdlet.WriteDebug(message);
    public void WriteWarning(string message) => Cmdlet.WriteWarning(message);
    public void WriteInformation(InformationRecord informationRecord) => Cmdlet.WriteInformation(informationRecord);
    public void WriteProgress(ProgressRecord progressRecord) => Cmdlet.WriteProgress(progressRecord);
  }
}
