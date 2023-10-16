using CollectionUtils.Test.CommandBuilders;
using CollectionUtils.Test.Utils;
using Markdig.Extensions.Tables;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class PropertyGetterTests
  {
    private static void AddObject(PowerShell shell)
    {
      shell.InvokeScript("class IdValuePair { [int] $Id; [string] $Value }");

      shell.InvokeScript("$obj = [IdValuePair]::new()");

      shell.InvokeScript("$obj.Id = 1");

      shell.InvokeScript("$obj.Value = 'one'");
    }

    private static void AddHashtable(PowerShell shell, bool wrapInPSObject = false)
    {
      shell.InvokeScript("$obj = @{ Id = 1; Value = 'one' }");

      if (wrapInPSObject)
      {
        shell.InvokeScript("function WrapInPSObject { param ( [PSObject]$value ) $value }");
        shell.InvokeScript("$obj = WrapInPSObject $obj");
      }
    }

    private static void AddPSCustomObject(PowerShell shell)
      => shell.InvokeScript("$obj = [pscustomobject]@{ Id = 1; Value = 'one' }");

    private static void AddGenericDictionary(PowerShell shell)
      => shell.InvokeScript("$obj = [System.Collections.Generic.Dictionary[string, object]]::new(); $obj.Add('Id', 1); $obj.Add('Value', 'one')");

    private static void AddDataTable(PowerShell shell) =>
      shell.InvokeScript(@"
        $table = [System.Data.DataTable]::new();
        $table.Columns.Add('Id', [int]);
        $table.Columns.Add('Value', [string]);
        $row = $table.Rows.Add();
        $row['Id'] = 1;
        $row['Value'] = 'one'");

    private static string GetPropertyScript(string propertyName)
      => $"[{typeof(PropertyGetter).FullName}]::GetProperty($obj, '{propertyName}')";

    private static string GetPropertyScriptBlock()
      => $"[{typeof(PropertyGetter).FullName}]::GetProperty($obj, {PSBuilder.KeyField("Id", "$_.Id")})";

    private static string GetRowPropertyScriptBlock()
      => $"[{typeof(PropertyGetter).FullName}]::GetProperty($table.Rows[0], {PSBuilder.KeyField("Id", "$_['Id']")})";

    private static object GetObject() => new { Id = 1, Value = "one" };

    private static DataTable GetDataTable()
    {
      var table = new DataTable();

      table.Columns.Add("Id", typeof(int));
      table.Columns.Add("Value", typeof(string));

      var row = table.Rows.Add();

      row["Id"] = 1;
      row["Value"] = "one";

      return table;
    }

    private static Dictionary<string, object> GetDictionary() =>
      new() { { "Id", 1 }, { "Value", "one" } };

    private static Hashtable GetHashtable() =>
      new() { { "Id", 1 }, { "Value", "one" } };

    [TestMethod]
    public void GetProperty_TypeIsObject()
    {
      // Arrange
      var obj = GetObject();

      // Act
      var result = PropertyGetter.GetProperty(obj, new KeyField("Id"));

      // Assert
      Assert.IsInstanceOfType<int>(result);
      Assert.AreEqual(1, result);
    }

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
      var table = GetDataTable();

      // Act
      var result = PropertyGetter.GetProperty(table.Rows[0], new KeyField("Id"));

      // Assert
      Assert.IsInstanceOfType<int>(result);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetProperty_TypeIsGenericDictionary()
    {
      // Arrange
      var dictionary = GetDictionary();

      // Act
      var result = PropertyGetter.GetProperty(dictionary, new KeyField("Id"));

      // Assert
      Assert.IsInstanceOfType<int>(result);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetProperty_TypeIsHashtable()
    {
      // Arrange
      var hashtable = GetHashtable();

      // Act
      var result = PropertyGetter.GetProperty(hashtable, new KeyField("Id"));

      // Assert
      Assert.IsInstanceOfType<int>(result);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetPropertyWithScriptBlock_TypeIsObject()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();
      AddObject(shell);

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

    [TestMethod]
    public void GetProperty_TypeIsObjectWrappedInPSObject()
    {
      // Arrange
      var obj = GetObject();
      var psObject = new PSObject(obj);

      // Act
      var result = PropertyGetter.GetProperty(psObject, new KeyField("Id"));

      // Assert
      Assert.IsInstanceOfType<int>(result);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetProperty_TypeIsDataRowWrappedInPSObject()
    {
      // Arrange
      var table = GetDataTable();
      var psObject = new PSObject(table.Rows[0]);

      // Act
      var result = PropertyGetter.GetProperty(psObject, new KeyField("Id"));

      // Assert
      Assert.IsInstanceOfType<int>(result);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetProperty_TypeIsGenericDictionaryWrappedInPSObject()
    {
      // Arrange
      var dictionary = GetDictionary();
      var psObject = new PSObject(dictionary);

      // Act
      var result = PropertyGetter.GetProperty(psObject, new KeyField("Id"));

      // Assert
      Assert.IsInstanceOfType<int>(result);
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetProperty_TypeIsHashtableWrappedInPSObject()
    {
      // Arrange
      var hashtable = GetHashtable();
      var psObject = new PSObject(hashtable);

      // Act
      var result = PropertyGetter.GetProperty(psObject, new KeyField("Id"));

      // Assert
      Assert.IsInstanceOfType<int>(result);
      Assert.AreEqual(1, result);
    }

  }
}