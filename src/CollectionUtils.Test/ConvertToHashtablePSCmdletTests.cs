using System.Collections;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class ConvertToHashtablePSCmdletTests
  {
    [TestMethod]
    public void Invoke_SingleKeyFieldOfTypeInt_KeyIsInt()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      var script = "ConvertTo-Hashtable -InputObject $objs -Key Id";

      // Act
      var output =
        shell
        .InvokeScript(script);

      var results =
        output
        .Cast<PSObject>()
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(2, results.Count);
      Assert.IsTrue(results.Cast<DictionaryEntry>().All(x => x.Key is int));
    }

    [TestMethod]
    public void Invoke_SingleKeyFieldOfTypeString_KeyIsString()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      var script = "ConvertTo-Hashtable -InputObject $objs -Key Value";

      // Act
      var output =
        shell
        .InvokeScript(script);

      var results =
        output
        .Cast<PSObject>()
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(2, results.Count);
      Assert.IsTrue(results.Cast<DictionaryEntry>().All(x => x.Key is string));
    }

    [TestMethod]
    public void Invoke_MultipleKeyFields_KeyIsHashtable()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      var script = "ConvertTo-Hashtable -InputObject $objs -Key Id, Value";

      // Act
      var output =
        shell
        .InvokeScript(script);

      var results =
        output
        .Cast<PSObject>()
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(2, results.Count);
      Assert.IsTrue(results.Cast<DictionaryEntry>().All(x => x.Key is Hashtable));
    }
  }
}
