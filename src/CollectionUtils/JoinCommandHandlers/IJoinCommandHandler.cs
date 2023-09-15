using System;
using System.Management.Automation;

namespace CollectionUtils.JoinCommandHandlers
{
  internal interface IJoinCommandHandler : IDisposable
  {
    void Next(PSObject left);

    void WriteRemainingObjects();
  }
}