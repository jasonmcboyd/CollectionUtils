using CollectionUtils.JoinCommandHandlers;
using CollectionUtils.Utilities;
using System.ComponentModel;
using System.Management.Automation;

namespace CollectionUtils
{
  internal static class PSCmdletKeyCollisionStrategySelector
  {
    public static KeyCollisionStrategy SelectStrategy(
      this KeyCollisionPreference keyCollisionPreference,
      PowerShellWriter powerShellWriter)
    {
      return keyCollisionPreference switch
      {
        KeyCollisionPreference.Error => (key, value) =>
        {
          powerShellWriter.WriteError(
            new ErrorRecord(
              new PSArgumentException(
                $"Key collision detected for key {key}."),
                "KeyCollision",
                ErrorCategory.InvalidArgument,
                null));

          // TODO:
          //_CancellationTokenSource.Cancel();
        },
        KeyCollisionPreference.Ignore => (key, value) => { },
        KeyCollisionPreference.Warn => (key, value) =>
        {
          powerShellWriter.WriteWarning($"Key collision detected for key {key}.");
        },
        _ => throw new InvalidEnumArgumentException(nameof(KeyCollisionPreference), (int)keyCollisionPreference, typeof(KeyCollisionPreference))
      };
    }
  }
}
