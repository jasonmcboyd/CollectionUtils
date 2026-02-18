using CollectionUtils.Test.Utils;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class TestCollectionPSCmdletTests
  {
    [TestMethod]
    public void TestCollection_Any_MatchFound_ReturnsTrue()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$items = @(1, 2, 3, 4, 5)");

      // Act
      var results = shell
        .InvokeScript("$items | Test-Collection { $_ -eq 3 } -Any")
        .ToArray();

      // Assert
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(true, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_Any_NoMatch_ReturnsFalse()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$items = @(1, 2, 3)");

      // Act
      var results = shell
        .InvokeScript("$items | Test-Collection { $_ -gt 10 } -Any")
        .ToArray();

      // Assert
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(false, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_All_AllMatch_ReturnsTrue()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$items = @(2, 4, 6)");

      // Act
      var results = shell
        .InvokeScript("$items | Test-Collection { $_ % 2 -eq 0 } -All")
        .ToArray();

      // Assert
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(true, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_All_NotAllMatch_ReturnsFalse()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$items = @(2, 3, 6)");

      // Act
      var results = shell
        .InvokeScript("$items | Test-Collection { $_ % 2 -eq 0 } -All")
        .ToArray();

      // Assert
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(false, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_Any_ShortCircuits_ReturnsExactlyOneResult()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      // Pipe 5 items where item at index 1 matches. With short-circuiting,
      // we should get exactly one True result, not multiple.
      shell.InvokeScript("$items = @(0, 1, 2, 3, 4)");

      // Act
      var results = shell
        .InvokeScript("$items | Test-Collection { $_ -eq 1 } -Any")
        .ToArray();

      // Assert - exactly one result
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(true, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_All_ShortCircuits_ReturnsExactlyOneResult()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      // Pipe 5 items where the first item (0) fails the predicate.
      // With short-circuiting, we should get exactly one False result.
      shell.InvokeScript("$items = @(0, 1, 2, 3, 4)");

      // Act
      var results = shell
        .InvokeScript("$items | Test-Collection { $_ -gt 0 } -All")
        .ToArray();

      // Assert - exactly one result
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(false, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_Any_EmptyCollection_ReturnsFalse()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      // Act
      var results = shell
        .InvokeScript("@() | Test-Collection { $true } -Any")
        .ToArray();

      // Assert
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(false, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_All_EmptyCollection_ReturnsTrue()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      // Act
      var results = shell
        .InvokeScript("@() | Test-Collection { $false } -All")
        .ToArray();

      // Assert - vacuously true
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(true, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_Any_FirstItemMatches_ReturnsTrue()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      // Act
      var results = shell
        .InvokeScript("@(1, 2, 3) | Test-Collection { $_ -eq 1 } -Any")
        .ToArray();

      // Assert
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(true, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_All_LastItemFails_ReturnsFalse()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      // Act
      var results = shell
        .InvokeScript("@(2, 4, 5) | Test-Collection { $_ % 2 -eq 0 } -All")
        .ToArray();

      // Assert
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(false, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_Any_SingleItemMatch_ReturnsTrue()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      // Act
      var results = shell
        .InvokeScript("@(42) | Test-Collection { $_ -eq 42 } -Any")
        .ToArray();

      // Assert
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(true, results[0].BaseObject);
    }

    [TestMethod]
    public void TestCollection_All_SingleItemMatch_ReturnsTrue()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      // Act
      var results = shell
        .InvokeScript("@(42) | Test-Collection { $_ -eq 42 } -All")
        .ToArray();

      // Assert
      Assert.AreEqual(1, results.Length);
      Assert.AreEqual(true, results[0].BaseObject);
    }
  }
}
