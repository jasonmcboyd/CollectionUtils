using CollectionUtils.Utilities;
using System;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.JoinCommandHandlers
{
  internal class CrossJoinCommandHandler : JoinCommandHandlerBase
  {
    public CrossJoinCommandHandler(
      object[] rightCollection,
      PowerShellWriter powerShellWriter,
      CancellationToken cancellationToken)
      : base(rightCollection, powerShellWriter, cancellationToken)
    {
    }

    public override void Next(PSObject left)
    {
      if (RightCollection.Length == 0)
        return;

      for (int i = 0; i < RightCollection.Length; i++)
      {
        if (CancellationToken.IsCancellationRequested)
          throw new OperationCanceledException();

        var psObject = CreatePSObject(left, RightCollection[i]);

        WriteObject(psObject);
      }
    }

    public override void WriteRemainingObjects()
    {
    }

    public override void Dispose()
    {
    }
  }
}
