using CollectionUtils.PSCmdlets;
using System.ComponentModel;

namespace CollectionUtils.JoinCommandHandlers
{
  internal static class JoinCollectionKeyCollisionPreferenceExtensions
  {
    public static KeyCollisionPreference ToKeyCollisionPreference(this JoinCollectionKeyCollisionPreference keyCollisionPreference)
    {
      return keyCollisionPreference switch
      {
        JoinCollectionKeyCollisionPreference.Error => KeyCollisionPreference.Error,
        JoinCollectionKeyCollisionPreference.Ignore => KeyCollisionPreference.Ignore,
        JoinCollectionKeyCollisionPreference.Warn => KeyCollisionPreference.Warn,
        _ => throw new InvalidEnumArgumentException(nameof(keyCollisionPreference), (int)keyCollisionPreference, typeof(JoinCollectionKeyCollisionPreference))
      };
    }
  }
}
