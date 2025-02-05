using System;
using CollectionUtils.Test.CommandBuilders;
using CollectionUtils.Test.Utils;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class ConvertFromSmartCsvPSCmdletTests
  {
    [TestMethod]
    public void Invoke_EachMemberTypeIsCorrect()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$csv = \"String, Date, Number, Decimal, Bool`r`nJason, 3/28/1980, 44, 123.456, true\"");

      var command = "ConvertFrom-SmartCsv -CsvInput $csv";

      // Act
      var results =
        shell
        .InvokeScript(command)
        .ToArray();

      var obj = results[0];

      Assert.AreEqual(1, results.Length);

      Assert.AreEqual(typeof(String).FullName,   obj.Properties["String"].TypeNameOfValue);
      Assert.AreEqual(typeof(DateTime).FullName, obj.Properties["Date"].TypeNameOfValue);
      Assert.AreEqual(typeof(Int32).FullName,    obj.Properties["Number"].TypeNameOfValue);
      Assert.AreEqual(typeof(Decimal).FullName,  obj.Properties["Decimal"].TypeNameOfValue);
      Assert.AreEqual(typeof(Boolean).FullName,  obj.Properties["Bool"].TypeNameOfValue);
    }

    [TestMethod]
    public void Invoke_ValueTypeIsString_ValueTextIsNull_NullIsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$csv = \"Value`r`nJason`r`nnull\"");

      var command = "ConvertFrom-SmartCsv -CsvInput $csv";

      // Act
      var results =
        shell
        .InvokeScript(command)
        .ToArray();

      Assert.AreEqual(2, results.Length);

      Assert.AreEqual(typeof(string).FullName, results[0].Properties["Value"].TypeNameOfValue);

      Assert.AreEqual("Jason", results[0].Properties["Value"].Value);
      Assert.IsNull(results[1].Properties["Value"].Value);
    }

    [TestMethod]
    public void Invoke_ValueTypeIsInt_ValueTextIsNull_NullIsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$csv = \"Value`r`n44`r`nnull\"");

      var command = "ConvertFrom-SmartCsv -CsvInput $csv";

      // Act
      var results =
        shell
        .InvokeScript(command)
        .ToArray();

      Assert.AreEqual(2, results.Length);

      Assert.AreEqual(typeof(int).FullName, results[0].Properties["Value"].TypeNameOfValue);

      Assert.AreEqual(44, results[0].Properties["Value"].Value);
      Assert.IsNull(results[1].Properties["Value"].Value);
    }
  }
}
