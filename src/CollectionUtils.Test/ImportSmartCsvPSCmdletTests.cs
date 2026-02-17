using System;
using System.IO;
using CollectionUtils.Test.Utils;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class ImportSmartCsvPSCmdletTests
  {
    [TestMethod]
    public void Invoke_AbsolutePath_ReturnsExpectedResults()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
      Directory.CreateDirectory(tempDir);

      var csvPath = Path.Combine(tempDir, "test.csv");
      File.WriteAllText(csvPath, "Name,Age\r\nAlice,30\r\nBob,25");

      try
      {
        var command = $"Import-SmartCsv -Path '{csvPath}'";

        // Act
        var results = shell.InvokeScript(command).ToArray();

        // Assert
        Assert.AreEqual(2, results.Length);
        Assert.AreEqual("Alice", results[0].Properties["Name"].Value);
        Assert.AreEqual(30, results[0].Properties["Age"].Value);
        Assert.AreEqual("Bob", results[1].Properties["Name"].Value);
        Assert.AreEqual(25, results[1].Properties["Age"].Value);
      }
      finally
      {
        Directory.Delete(tempDir, true);
      }
    }

    [TestMethod]
    public void Invoke_RelativePathAfterSetLocation_ResolvesAgainstPSWorkingDirectory()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
      Directory.CreateDirectory(tempDir);

      var csvPath = Path.Combine(tempDir, "data.csv");
      File.WriteAllText(csvPath, "Name,Value\r\nfoo,42");

      try
      {
        // Change PowerShell's $PWD to the temp directory.
        // This diverges from .NET's Environment.CurrentDirectory.
        shell.InvokeScript($"Set-Location '{tempDir}'");

        var command = "Import-SmartCsv -Path './data.csv'";

        // Act
        var results = shell.InvokeScript(command).ToArray();

        // Assert — before the fix, this would throw FileNotFoundException
        // because the relative path resolved against .NET's working directory
        // instead of PowerShell's $PWD.
        Assert.AreEqual(1, results.Length);
        Assert.AreEqual("foo", results[0].Properties["Name"].Value);
        Assert.AreEqual(42, results[0].Properties["Value"].Value);
      }
      finally
      {
        Directory.Delete(tempDir, true);
      }
    }

    [TestMethod]
    public void Invoke_RelativePathViaPipeline_ResolvesAgainstPSWorkingDirectory()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
      Directory.CreateDirectory(tempDir);

      File.WriteAllText(Path.Combine(tempDir, "a.csv"), "Id\r\n1");
      File.WriteAllText(Path.Combine(tempDir, "b.csv"), "Id\r\n2");

      try
      {
        shell.InvokeScript($"Set-Location '{tempDir}'");

        var command = "'./a.csv', './b.csv' | Import-SmartCsv";

        // Act
        var results = shell.InvokeScript(command).ToArray();

        // Assert
        Assert.AreEqual(2, results.Length);
        Assert.AreEqual(1, results[0].Properties["Id"].Value);
        Assert.AreEqual(2, results[1].Properties["Id"].Value);
      }
      finally
      {
        Directory.Delete(tempDir, true);
      }
    }
  }
}
