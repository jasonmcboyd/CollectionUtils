using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using CollectionUtils.Test.Utils;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class ImportSmartExcelPSCmdletTests
  {
    [ClassInitialize]
    public static void ClassInit(TestContext context)
    {
      // ExcelDataReader requires the CodePages encoding provider for .xlsx parsing.
      // In a full PowerShell session this is already registered, but in the
      // test harness we must do it explicitly.
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    /// <summary>
    /// Creates a minimal .xlsx file with the given shared strings and sheet XML.
    /// </summary>
    private static void CreateXlsx(string path, string sheetXml, string sharedStringsXml)
    {
      var contentTypes = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Types xmlns=""http://schemas.openxmlformats.org/package/2006/content-types"">
  <Default Extension=""rels"" ContentType=""application/vnd.openxmlformats-package.relationships+xml""/>
  <Default Extension=""xml"" ContentType=""application/xml""/>
  <Override PartName=""/xl/workbook.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml""/>
  <Override PartName=""/xl/worksheets/sheet1.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml""/>
  <Override PartName=""/xl/sharedStrings.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml""/>
</Types>";

      var rels = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"" Target=""xl/workbook.xml""/>
</Relationships>";

      var wbRels = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"" Target=""worksheets/sheet1.xml""/>
  <Relationship Id=""rId2"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings"" Target=""sharedStrings.xml""/>
</Relationships>";

      var workbook = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<workbook xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main""
          xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships"">
  <sheets>
    <sheet name=""Sheet1"" sheetId=""1"" r:id=""rId1""/>
  </sheets>
</workbook>";

      using var fs = new FileStream(path, FileMode.Create);
      using var archive = new ZipArchive(fs, ZipArchiveMode.Create);

      AddEntry(archive, "[Content_Types].xml", contentTypes);
      AddEntry(archive, "_rels/.rels", rels);
      AddEntry(archive, "xl/_rels/workbook.xml.rels", wbRels);
      AddEntry(archive, "xl/workbook.xml", workbook);
      AddEntry(archive, "xl/worksheets/sheet1.xml", sheetXml);
      AddEntry(archive, "xl/sharedStrings.xml", sharedStringsXml);
    }

    private static void AddEntry(ZipArchive archive, string entryName, string content)
    {
      var entry = archive.CreateEntry(entryName);
      using var stream = entry.Open();
      var bytes = Encoding.UTF8.GetBytes(content);
      stream.Write(bytes, 0, bytes.Length);
    }

    [TestMethod]
    public void Invoke_NullHeaderCell_UsesFallbackColumnName()
    {
      // Arrange: Create an Excel file where column B header is empty/null.
      // Before the fix, reader[columnIndex].ToString() threw NullReferenceException.
      // After the fix, null headers get a fallback name like "Column1".
      using var shell = PowerShellUtilities.CreateShell();

      var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
      Directory.CreateDirectory(tempDir);

      var xlsxPath = Path.Combine(tempDir, "null_header.xlsx");

      // Sheet with 3 columns: A1="Name", B1=empty, C1="Age"
      // Data row: A2="Alice", B2=empty, C2=30
      var sheetXml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<worksheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"">
  <dimension ref=""A1:C2""/>
  <sheetData>
    <row r=""1"">
      <c r=""A1"" t=""s""><v>0</v></c>
      <c r=""C1"" t=""s""><v>1</v></c>
    </row>
    <row r=""2"">
      <c r=""A2"" t=""s""><v>2</v></c>
      <c r=""C2""><v>30</v></c>
    </row>
  </sheetData>
</worksheet>";

      var sharedStringsXml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<sst xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" count=""3"" uniqueCount=""3"">
  <si><t>Name</t></si>
  <si><t>Age</t></si>
  <si><t>Alice</t></si>
</sst>";

      CreateXlsx(xlsxPath, sheetXml, sharedStringsXml);

      try
      {
        var command = $"Import-SmartExcel -Path '{xlsxPath}'";

        // Act
        var results = shell.InvokeScript(command).ToArray();

        // Check for errors
        if (shell.HadErrors)
        {
          var errors = string.Join("; ", shell.Streams.Error.Select(e => e.ToString()));
          Assert.Fail($"PowerShell command had errors: {errors}");
        }

        // Assert — should not throw NullReferenceException
        Assert.AreEqual(1, results.Length, $"Expected 1 worksheet result, got {results.Length}");

        var worksheet = results[0];
        Assert.AreEqual("Sheet1", worksheet.Properties["WorksheetName"].Value);

        var data = (PSObject[])worksheet.Properties["Data"].Value;
        Assert.AreEqual(1, data.Length);

        var row = data[0];
        Assert.AreEqual("Alice", row.Properties["Name"].Value);
        Assert.AreEqual(30, row.Properties["Age"].Value);

        // The null header column should have a fallback name
        Assert.IsNotNull(row.Properties["Column1"],
          "Null header cell should be assigned fallback name 'Column1'");
      }
      finally
      {
        Directory.Delete(tempDir, true);
      }
    }

    [TestMethod]
    public void Invoke_EmptyWorksheet_DoesNotThrow()
    {
      // Arrange: Create an Excel file with a completely empty worksheet (no rows).
      // Before the fix, reader.Read() returned false but the result was discarded,
      // causing subsequent reader[columnIndex] access to throw.
      using var shell = PowerShellUtilities.CreateShell();

      var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
      Directory.CreateDirectory(tempDir);

      var xlsxPath = Path.Combine(tempDir, "empty_sheet.xlsx");

      // Sheet with empty sheetData - no rows at all
      var sheetXml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<worksheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"">
  <sheetData/>
</worksheet>";

      var sharedStringsXml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<sst xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" count=""0"" uniqueCount=""0"">
</sst>";

      CreateXlsx(xlsxPath, sheetXml, sharedStringsXml);

      try
      {
        var command = $"Import-SmartExcel -Path '{xlsxPath}'";

        // Act - should not throw
        var results = shell.InvokeScript(command).ToArray();

        // Check for errors
        if (shell.HadErrors)
        {
          var errors = string.Join("; ", shell.Streams.Error.Select(e => e.ToString()));
          Assert.Fail($"PowerShell command had errors: {errors}");
        }

        // Assert - empty sheet should produce no worksheet results
        Assert.AreEqual(0, results.Length,
          $"Expected 0 worksheet results for empty sheet, got {results.Length}");
      }
      finally
      {
        Directory.Delete(tempDir, true);
      }
    }
  }
}
