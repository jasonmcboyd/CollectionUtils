using System.Management.Automation;

namespace CollectionUtils
{
  public class KeyField
  {
    public KeyField(string property)
    {
      Property = property;
    }

    public KeyField(
      string property,
      ScriptBlock expression)
    {
      Property = property;
      Expression = expression;
    }

    public string Property { get; set; }
    public ScriptBlock? Expression { get; set; }
  }
}
