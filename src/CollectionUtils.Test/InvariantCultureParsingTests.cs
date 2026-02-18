using System;
using System.Globalization;
using System.Threading;
using CollectionUtils.Test.Utils;

namespace CollectionUtils.Test
{
  [TestClass]
  public class InvariantCultureParsingTests
  {
    /// <summary>
    /// Verifies that ConvertFrom-SmartCsv correctly parses decimals using
    /// invariant culture (period as decimal separator) even when the
    /// current thread culture uses a comma as the decimal separator.
    /// </summary>
    [TestMethod]
    public void ConvertFromSmartCsv_DecimalWithPeriodSeparator_ParsedCorrectlyUnderNonEnglishCulture()
    {
      var originalCulture = Thread.CurrentThread.CurrentCulture;

      try
      {
        // Set culture to German, which uses comma as decimal separator
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

        using var shell = PowerShellUtilities.CreateShell();

        shell.InvokeScript("$csv = \"Value`r`n123.456\"");

        var results = shell.InvokeScript("ConvertFrom-SmartCsv -CsvInput $csv").ToArray();

        Assert.AreEqual(1, results.Length);
        Assert.AreEqual(typeof(Decimal).FullName, results[0].Properties["Value"].TypeNameOfValue);
        Assert.AreEqual(123.456m, results[0].Properties["Value"].Value);
      }
      finally
      {
        Thread.CurrentThread.CurrentCulture = originalCulture;
      }
    }

    /// <summary>
    /// Verifies that ConvertFrom-SmartCsv correctly infers integer type
    /// under a culture where the value might otherwise be ambiguous.
    /// </summary>
    [TestMethod]
    public void ConvertFromSmartCsv_IntegerColumn_ParsedCorrectlyUnderNonEnglishCulture()
    {
      var originalCulture = Thread.CurrentThread.CurrentCulture;

      try
      {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

        using var shell = PowerShellUtilities.CreateShell();

        shell.InvokeScript("$csv = \"Value`r`n42`r`n100\"");

        var results = shell.InvokeScript("ConvertFrom-SmartCsv -CsvInput $csv").ToArray();

        Assert.AreEqual(2, results.Length);
        Assert.AreEqual(typeof(Int32).FullName, results[0].Properties["Value"].TypeNameOfValue);
        Assert.AreEqual(42, results[0].Properties["Value"].Value);
        Assert.AreEqual(100, results[1].Properties["Value"].Value);
      }
      finally
      {
        Thread.CurrentThread.CurrentCulture = originalCulture;
      }
    }

    /// <summary>
    /// Verifies that ConvertFrom-SmartCsv correctly parses ISO-style dates
    /// under a non-English culture.
    /// </summary>
    [TestMethod]
    public void ConvertFromSmartCsv_DateColumn_ParsedCorrectlyUnderNonEnglishCulture()
    {
      var originalCulture = Thread.CurrentThread.CurrentCulture;

      try
      {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

        using var shell = PowerShellUtilities.CreateShell();

        // Use an unambiguous date format that invariant culture parses
        shell.InvokeScript("$csv = \"Value`r`n2024-03-15\"");

        var results = shell.InvokeScript("ConvertFrom-SmartCsv -CsvInput $csv").ToArray();

        Assert.AreEqual(1, results.Length);
        Assert.AreEqual(typeof(DateTime).FullName, results[0].Properties["Value"].TypeNameOfValue);

        var date = (DateTime)results[0].Properties["Value"].Value;
        Assert.AreEqual(2024, date.Year);
        Assert.AreEqual(3, date.Month);
        Assert.AreEqual(15, date.Day);
      }
      finally
      {
        Thread.CurrentThread.CurrentCulture = originalCulture;
      }
    }

    /// <summary>
    /// Verifies that a CSV with mixed types (string, decimal, date, int,
    /// bool) is inferred and parsed correctly under a non-English culture.
    /// </summary>
    [TestMethod]
    public void ConvertFromSmartCsv_MixedTypes_AllParsedCorrectlyUnderNonEnglishCulture()
    {
      var originalCulture = Thread.CurrentThread.CurrentCulture;

      try
      {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");

        using var shell = PowerShellUtilities.CreateShell();

        shell.InvokeScript(
          "$csv = \"Name,Amount,Date,Count,Active`r`n" +
          "Alice,99.95,2024-01-20,7,true\"");

        var results = shell.InvokeScript("ConvertFrom-SmartCsv -CsvInput $csv").ToArray();

        Assert.AreEqual(1, results.Length);

        var obj = results[0];

        Assert.AreEqual(typeof(String).FullName,   obj.Properties["Name"].TypeNameOfValue);
        Assert.AreEqual(typeof(Decimal).FullName,  obj.Properties["Amount"].TypeNameOfValue);
        Assert.AreEqual(typeof(DateTime).FullName, obj.Properties["Date"].TypeNameOfValue);
        Assert.AreEqual(typeof(Int32).FullName,    obj.Properties["Count"].TypeNameOfValue);
        Assert.AreEqual(typeof(Boolean).FullName,  obj.Properties["Active"].TypeNameOfValue);

        Assert.AreEqual("Alice", obj.Properties["Name"].Value);
        Assert.AreEqual(99.95m,  obj.Properties["Amount"].Value);
        Assert.AreEqual(7,       obj.Properties["Count"].Value);
        Assert.AreEqual(true,    obj.Properties["Active"].Value);
      }
      finally
      {
        Thread.CurrentThread.CurrentCulture = originalCulture;
      }
    }
  }
}
