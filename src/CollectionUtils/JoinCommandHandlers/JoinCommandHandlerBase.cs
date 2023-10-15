using CollectionUtils.Utilities;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.JoinCommandHandlers
{
  internal abstract class JoinCommandHandlerBase : IJoinCommandHandler
  {
    public JoinCommandHandlerBase(
      object[] rightCollection,
      PowerShellWriter powerShellWriter,
      CancellationToken cancellationToken)
    {
      RightCollection = rightCollection;
      PowerShellWriter = powerShellWriter;
      CancellationToken = cancellationToken;
    }

    protected object[] RightCollection { get; }
    protected PowerShellWriter PowerShellWriter { get; }
    protected CancellationToken CancellationToken { get; }

    public abstract void Next(PSObject left);

    public abstract void WriteRemainingObjects();

    protected virtual PSObject CreatePSObject(object? left, object? right)
    {
      var psObject = new PSObject();

      psObject.Properties.Add(new PSNoteProperty("Left", left));
      psObject.Properties.Add(new PSNoteProperty("Right", right));

      return psObject;
    }

    public abstract void Dispose();
  }
}
