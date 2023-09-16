using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils
{
  public class KeyParameter : IEnumerable<KeyField>
  {
    public KeyParameter(string property)
    {
      KeyFields = new List<KeyField> { new KeyField(property) }.AsReadOnly();
    }

    public KeyParameter(Hashtable hashtable)
    {
      KeyFields =
        hashtable
        .Cast<DictionaryEntry>()
        .Select(entry => new KeyField((string)entry.Key, (ScriptBlock)entry.Value))
        .ToList()
        .AsReadOnly();
    }

    public IReadOnlyList<KeyField> KeyFields { get; }

    public IEnumerator<KeyField> GetEnumerator() => KeyFields.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
