using System;
using CollectionUtils.Test.Utils;

namespace CollectionUtils.Test
{
  [TestClass]
  public class BooleanNumericInferenceTests
  {
    /// <summary>
    /// Verifies that a CSV column containing only "1" and "0" values
    /// is inferred as Boolean and parsed to true/false.
    /// This is the primary regression test for bug #24.
    /// </summary>
    [TestMethod]
    public void ConvertFromSmartCsv_ColumnWithOnlyOneAndZero_InferredAsBoolean()
    {
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$csv = \"Active`r`n1`r`n0`r`n1\"");

      var results = shell.InvokeScript("ConvertFrom-SmartCsv -CsvInput $csv").ToArray();

      Assert.AreEqual(3, results.Length);

      // All values should be Boolean type
      Assert.AreEqual(typeof(Boolean).FullName, results[0].Properties["Active"].TypeNameOfValue);
      Assert.AreEqual(typeof(Boolean).FullName, results[1].Properties["Active"].TypeNameOfValue);
      Assert.AreEqual(typeof(Boolean).FullName, results[2].Properties["Active"].TypeNameOfValue);

      // "1" should parse to true, "0" should parse to false
      Assert.AreEqual(true,  results[0].Properties["Active"].Value);
      Assert.AreEqual(false, results[1].Properties["Active"].Value);
      Assert.AreEqual(true,  results[2].Properties["Active"].Value);
    }

    /// <summary>
    /// Verifies that a CSV column with "1", "0", and other integer values
    /// is still inferred as Int32, not Boolean.
    /// </summary>
    [TestMethod]
    public void ConvertFromSmartCsv_ColumnWithZeroOneAndOtherIntegers_InferredAsInt32()
    {
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$csv = \"Value`r`n0`r`n1`r`n2\"");

      var results = shell.InvokeScript("ConvertFrom-SmartCsv -CsvInput $csv").ToArray();

      Assert.AreEqual(3, results.Length);

      Assert.AreEqual(typeof(Int32).FullName, results[0].Properties["Value"].TypeNameOfValue);
      Assert.AreEqual(0, results[0].Properties["Value"].Value);
      Assert.AreEqual(1, results[1].Properties["Value"].Value);
      Assert.AreEqual(2, results[2].Properties["Value"].Value);
    }

    /// <summary>
    /// Verifies that a CSV column with "1", "0", and null values
    /// is inferred as Boolean (nulls are ignored during inference).
    /// </summary>
    [TestMethod]
    public void ConvertFromSmartCsv_ColumnWithZeroOneAndNull_InferredAsBoolean()
    {
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$csv = \"Active`r`n1`r`nnull`r`n0\"");

      var results = shell.InvokeScript("ConvertFrom-SmartCsv -CsvInput $csv").ToArray();

      Assert.AreEqual(3, results.Length);

      Assert.AreEqual(typeof(Boolean).FullName, results[0].Properties["Active"].TypeNameOfValue);
      Assert.AreEqual(true,  results[0].Properties["Active"].Value);
      Assert.IsNull(results[1].Properties["Active"].Value);
      Assert.AreEqual(false, results[2].Properties["Active"].Value);
    }

    /// <summary>
    /// Verifies that traditional boolean values ("true"/"false") still work
    /// correctly after the "1"/"0" fix.
    /// </summary>
    [TestMethod]
    public void ConvertFromSmartCsv_ColumnWithTrueFalseStrings_StillInferredAsBoolean()
    {
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$csv = \"Active`r`ntrue`r`nfalse\"");

      var results = shell.InvokeScript("ConvertFrom-SmartCsv -CsvInput $csv").ToArray();

      Assert.AreEqual(2, results.Length);

      Assert.AreEqual(typeof(Boolean).FullName, results[0].Properties["Active"].TypeNameOfValue);
      Assert.AreEqual(true,  results[0].Properties["Active"].Value);
      Assert.AreEqual(false, results[1].Properties["Active"].Value);
    }
  }
}
