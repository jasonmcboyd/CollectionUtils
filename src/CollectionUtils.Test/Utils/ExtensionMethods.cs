using System.Collections;
using System.Collections.Generic;

namespace CollectionUtils.Test.Utils
{
  internal static class ExtensionMethods
  {
    public static string JoinStrings(this IEnumerable<string> strings, string separator) => string.Join(separator, strings);

    public static T Get<T>(this Hashtable hashtable, object key) => (T)hashtable[key]!;
  }
}
