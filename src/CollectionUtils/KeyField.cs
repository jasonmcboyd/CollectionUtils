using System.Management.Automation;

namespace CollectionUtils
{
  public class KeyField
  {
    public KeyField()
    {
    }

    public KeyField(string property)
    {
      Property = property;
    }

    public KeyField(
      string property,
      ScriptBlock script)
    {
      Property = property;
      Script = script;
    }

    public string Property { get; set; } = default!;
    public ScriptBlock? Script { get; set; }
  }
}
