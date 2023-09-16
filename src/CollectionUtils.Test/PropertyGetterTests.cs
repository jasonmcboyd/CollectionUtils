using CollectionUtils.Test.CommandBuilders;
using CollectionUtils.Test.Utils;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class PropertyGetterTests
  {
    private void AddHashtable(PowerShell shell)
      => shell.InvokeScript("$obj = @{ Id = 1; Value = 'one' }");

    private void AddPSCustomObject(PowerShell shell)
      => shell.InvokeScript("$obj = [pscustomobject]@{ Id = 1; Value = 'one' }");

    private void AddGenericDictionary(PowerShell shell)
      => shell.InvokeScript("$obj = [System.Collections.Generic.Dictionary[string, object]]::new(); $obj.Add('Id', 1); $obj.Add('Value', 'one')");

    private void AddDataTable(PowerShell shell) =>
      shell.InvokeScript(@"
        $table = [System.Data.DataTable]::new();
        $table.Columns.Add('Id', [int]);
        $table.Columns.Add('Value', [string]);
        $row = $table.Rows.Add();
        $row['Id'] = 1;
        $row['Value'] = 'one'");

    private string GetRowPropertyScript(string propertyName)
      => $"[{typeof(PropertyGetter).FullName}]::GetProperty($table.Rows[0], '{propertyName}')";

    private string GetPropertyScript(string propertyName)
      => $"[{typeof(PropertyGetter).FullName}]::GetProperty($obj, '{propertyName}')";

    private string GetPropertyScriptBlock()
      => $"[{typeof(PropertyGetter).FullName}]::GetProperty($obj, {PSBuilder.KeyField("Id", "$_.Id")})";

    private string GetRowPropertyScriptBlock()
      => $"[{typeof(PropertyGetter).FullName}]::GetProperty($table.Rows[0], {PSBuilder.KeyField("Id", "$_['Id']")})";

    [TestMethod]
    public void GetProperty_TypeIsPSCustomObject()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();
      AddPSCustomObject(shell);

      // Act
      var output =
        shell
        .InvokeScript(GetPropertyScript("Id"));

      var result =
        output
        .Cast<PSObject>()
        .Single();

      // Assert
      Assert.IsTrue(result.BaseObject is int);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetProperty_TypeIsDataRow()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();
      AddDataTable(shell);

      // Act
      var output =
        shell
        .InvokeScript(GetRowPropertyScript("Id"));

      var result =
        output
        .Cast<PSObject>()
        .Single();

      // Assert
      Assert.IsTrue(result.BaseObject is int);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetProperty_TypeIsGenericDictionary()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();
      AddGenericDictionary(shell);

      // Act
      var result =
        shell
        .InvokeScript(GetPropertyScript("Id"))
        .Cast<PSObject>()
        .Single();

      // Assert
      Assert.IsTrue(result.BaseObject is int);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetProperty_TypeIsHashtable()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();
      AddHashtable(shell);

      // Act
      var result =
        shell
        .InvokeScript(GetPropertyScript("Id"))
        .Cast<PSObject>()
        .Single();

      // Assert
      Assert.IsTrue(result.BaseObject is int);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetPropertyWithScriptBlock_TypeIsPSCustomObject()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();
      AddPSCustomObject(shell);

      // Act
      var output =
        shell
        .InvokeScript(GetPropertyScriptBlock());

      var result =
        output
        .Cast<PSObject>()
        .Single();

      // Assert
      Assert.IsTrue(result.BaseObject is int);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetPropertyWithScriptBlock_TypeIsGenericDictionary()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();
      AddGenericDictionary(shell);

      // Act
      var result =
        shell
        .InvokeScript(GetPropertyScriptBlock())
        .Cast<PSObject>()
        .Single();

      // Assert
      Assert.IsTrue(result.BaseObject is int);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetPropertyWithScriptBlock_TypeIsHashtable()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();
      AddHashtable(shell);

      // Act
      var result =
        shell
        .InvokeScript(GetPropertyScriptBlock())
        .Cast<PSObject>()
        .Single();

      // Assert
      Assert.IsTrue(result.BaseObject is int);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetPropertyWithScriptBlock_TypeIsDataRow()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();
      AddDataTable(shell);

      // Act
      var output =
        shell
        .InvokeScript(GetRowPropertyScriptBlock());

      var result =
        output
        .Cast<PSObject>()
        .Single();

      // Assert
      Assert.IsTrue(result.BaseObject is int);
      Assert.AreEqual(1, result);
    }
  }
}