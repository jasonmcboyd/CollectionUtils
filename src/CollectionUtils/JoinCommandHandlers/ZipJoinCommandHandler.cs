﻿using CollectionUtils.Utilities;
using System;
using System.Management.Automation;
using System.Threading;

namespace CollectionUtils.JoinCommandHandlers
{
  internal class ZipJoinCommandHandler : JoinCommandHandlerBase
  {
    public ZipJoinCommandHandler(
      object[] rightCollection,
      PowerShellWriter powerShellWriter,
      CancellationToken cancellationToken)
      : base(rightCollection, powerShellWriter, cancellationToken)
    {
    }

    private int _CurrentIndex = 0;

    public override void Next(PSObject left)
    {
      if (CancellationToken.IsCancellationRequested)
        throw new OperationCanceledException();

      object? right = null;

      if (_CurrentIndex < RightCollection.Length)
        right = RightCollection[_CurrentIndex++];

      var psObject = CreatePSObject(left, right);

      PowerShellWriter.WriteObject(psObject);
    }

    public override void WriteRemainingObjects()
    {
      if (_CurrentIndex >= RightCollection.Length)
        return;

      var span = RightCollection.AsSpan(_CurrentIndex);

      for (int i = 0; i < span.Length; i++)
      {
        if (CancellationToken.IsCancellationRequested)
          throw new OperationCanceledException();

        var psObject = CreatePSObject(null, span[i]);

        PowerShellWriter.WriteObject(psObject);
      }
    }

    public override void Dispose()
    {
    }
  }
}
