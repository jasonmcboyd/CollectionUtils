using System;
using CollectionUtils.Test.Utils;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class ConvertPropertyPSCmdletTests
  {
    [TestMethod]
    public void ConvertProperty_ValidConversion_ConvertsSuccessfully()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @([pscustomobject]@{ Name = 'Alice'; Age = '30' })");

      // Act
      var results = shell
        .InvokeScript("$objs | Convert-Property -Property Age -Type Int32")
        .ToArray();

      // Assert
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(typeof(Int32).FullName, results[0].Properties["Age"].TypeNameOfValue);
      Assert.AreEqual(30, results[0].Properties["Age"].Value);
      Assert.IsFalse(shell.HadErrors, "Expected no errors for valid conversion.");
    }

    [TestMethod]
    public void ConvertProperty_InvalidValue_WritesNonTerminatingErrorAndContinues()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript(
        "$objs = @(" +
        "[pscustomobject]@{ Name = 'Alice'; Age = '30' }, " +
        "[pscustomobject]@{ Name = 'Bob'; Age = 'notanumber' }, " +
        "[pscustomobject]@{ Name = 'Carol'; Age = '25' })");

      // Act
      var results = shell
        .InvokeScript("$objs | Convert-Property -Property Age -Type Int32")
        .ToArray();

      // Assert - all 3 objects should come through the pipeline
      Assert.AreEqual(3, results.Length, "All objects should pass through the pipeline.");

      // First and third should be converted
      Assert.AreEqual(typeof(Int32).FullName, results[0].Properties["Age"].TypeNameOfValue);
      Assert.AreEqual(30, results[0].Properties["Age"].Value);
      Assert.AreEqual("Alice", results[0].Properties["Name"].Value);

      Assert.AreEqual(typeof(Int32).FullName, results[2].Properties["Age"].TypeNameOfValue);
      Assert.AreEqual(25, results[2].Properties["Age"].Value);
      Assert.AreEqual("Carol", results[2].Properties["Name"].Value);

      // Second should be passed through unchanged (original string value)
      Assert.AreEqual("notanumber", results[1].Properties["Age"].Value);
      Assert.AreEqual("Bob", results[1].Properties["Name"].Value);

      // Should have a non-terminating error
      Assert.IsTrue(shell.HadErrors, "Expected a non-terminating error for invalid value.");
      Assert.AreEqual(1, shell.Streams.Error.Count, "Expected exactly one error record.");
      StringAssert.Contains(shell.Streams.Error[0].FullyQualifiedErrorId, "ConversionFailed");
    }

    [TestMethod]
    public void ConvertProperty_OverflowValue_WritesNonTerminatingErrorAndContinues()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      // Int32 max is 2,147,483,647; this exceeds it
      shell.InvokeScript(
        "$objs = @(" +
        "[pscustomobject]@{ Value = '100' }, " +
        "[pscustomobject]@{ Value = '99999999999999' }, " +
        "[pscustomobject]@{ Value = '200' })");

      // Act
      var results = shell
        .InvokeScript("$objs | Convert-Property -Property Value -Type Int32")
        .ToArray();

      // Assert
      Assert.AreEqual(3, results.Length, "All objects should pass through the pipeline.");

      Assert.AreEqual(typeof(Int32).FullName, results[0].Properties["Value"].TypeNameOfValue);
      Assert.AreEqual(100, results[0].Properties["Value"].Value);

      // Overflow value should be passed through unchanged
      Assert.AreEqual("99999999999999", results[1].Properties["Value"].Value);

      Assert.AreEqual(typeof(Int32).FullName, results[2].Properties["Value"].TypeNameOfValue);
      Assert.AreEqual(200, results[2].Properties["Value"].Value);

      Assert.IsTrue(shell.HadErrors, "Expected a non-terminating error for overflow value.");
      Assert.AreEqual(1, shell.Streams.Error.Count, "Expected exactly one error record.");
    }

    [TestMethod]
    public void ConvertProperty_AllValuesInvalid_WritesErrorForEachAndPassesAllThrough()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript(
        "$objs = @(" +
        "[pscustomobject]@{ Value = 'abc' }, " +
        "[pscustomobject]@{ Value = 'def' })");

      // Act
      var results = shell
        .InvokeScript("$objs | Convert-Property -Property Value -Type Int32")
        .ToArray();

      // Assert - both should pass through unchanged
      Assert.AreEqual(2, results.Length);
      Assert.AreEqual("abc", results[0].Properties["Value"].Value);
      Assert.AreEqual("def", results[1].Properties["Value"].Value);

      Assert.IsTrue(shell.HadErrors);
      Assert.AreEqual(2, shell.Streams.Error.Count, "Expected one error per failed conversion.");
    }

    [TestMethod]
    public void ConvertProperty_MissingProperty_PassesThroughUnchanged()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript(
        "$objs = @([pscustomobject]@{ Name = 'Alice' })");

      // Act
      var results = shell
        .InvokeScript("$objs | Convert-Property -Property Age -Type Int32")
        .ToArray();

      // Assert - object should pass through since property doesn't exist
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual("Alice", results[0].Properties["Name"].Value);
      Assert.IsFalse(shell.HadErrors, "Expected no errors for missing property.");
    }
  }
}
