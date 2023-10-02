using System;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.JoinCommandHandlers
{
  internal abstract class JoinCommandHandlerBase : IJoinCommandHandler
  {
    public JoinCommandHandlerBase(
      object[] rightCollection,
      Action<object> objectWriter,
      Action<ErrorRecord> errorWriter,
      CancellationToken cancellationToken)
    {
      RightCollection = rightCollection;
      ObjectWriter = objectWriter;
      ErrorWriter = errorWriter;
      CancellationToken = cancellationToken;
    }

    protected object[] RightCollection { get; }
    private Action<object> ObjectWriter { get; }
    private Action<ErrorRecord> ErrorWriter { get; }
    protected CancellationToken CancellationToken { get; }

    protected void WriteObject(PSObject pSObject) => ObjectWriter(pSObject);
    protected void WriteError(ErrorRecord errorRecord) => ErrorWriter(errorRecord);

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
