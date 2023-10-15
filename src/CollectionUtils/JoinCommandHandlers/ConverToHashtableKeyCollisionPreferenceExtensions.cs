using CollectionUtils.PSCmdlets;
using System.ComponentModel;

namespace CollectionUtils.JoinCommandHandlers
{
  internal static class ConvertToHashtableKeyCollisionPreferenceExtensions
  {
    public static KeyCollisionPreference ToKeyCollisionPreference(this ConvertToHashtableKeyCollisionPreference keyCollisionPreference)
    {
      return keyCollisionPreference switch
      {
        ConvertToHashtableKeyCollisionPreference.Error => KeyCollisionPreference.Error,
        ConvertToHashtableKeyCollisionPreference.Ignore => KeyCollisionPreference.Ignore,
        ConvertToHashtableKeyCollisionPreference.Warn => KeyCollisionPreference.Warn,
        _ => throw new InvalidEnumArgumentException(nameof(keyCollisionPreference), (int)keyCollisionPreference, typeof(ConvertToHashtableKeyCollisionPreference))
      };
    }
  }
}
