using CollectionUtils.Test.CommandBuilders;
using CollectionUtils.Test.Utils;
using CollectionUtils.JoinCommandHandlers;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class ValidationErrorStopsProcessingTests
  {
    [TestMethod]
    public void ConvertToHashtable_InvalidComparerPropertyName_ProducesErrorAndNoOutput()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      // Use a Comparer with a property name that is not in the Key
      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Id")
        .Comparer("@{ NonExistent = [System.StringComparer]::Ordinal }");

      // Act
      var output = shell.InvokeCommandBuilder(command);

      // Assert - validation error should prevent any output
      Assert.IsTrue(shell.HadErrors, "Expected an error for invalid comparer property name.");
      Assert.AreEqual(0, output.Count, "Expected no output when validation fails.");

      Assert.IsTrue(shell.Streams.Error.Count > 0, "Expected at least one error record.");
      var errorRecord = shell.Streams.Error[0];
      StringAssert.Contains(errorRecord.FullyQualifiedErrorId, "ComparerPropertyNameNotInKey");
    }

    [TestMethod]
    public void JoinCollection_InvalidComparerPropertyName_ProducesErrorAndNoOutput()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @( @{ Id = 1; Value = 'left' } )");
      shell.InvokeScript("$right = @( @{ Id = 1; Value = 'right' } )");

      // Use a Comparer with a property name that is not in the Key
      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .Key("Id")
        .Comparer("@{ NonExistent = [StringComparer]::OrdinalIgnoreCase }")
        .KeyedJoin(KeyedJoinType.Inner);

      // Act
      var output = shell.InvokeCommandBuilder(command);

      // Assert - validation error should prevent any output
      Assert.IsTrue(shell.HadErrors, "Expected an error for invalid comparer property name.");
      Assert.AreEqual(0, output.Count, "Expected no output when validation fails.");

      Assert.IsTrue(shell.Streams.Error.Count > 0, "Expected at least one error record.");
      var errorRecord = shell.Streams.Error[0];
      StringAssert.Contains(errorRecord.FullyQualifiedErrorId, "ComparerPropertyNameNotInKey");
    }

    [TestMethod]
    public void JoinCollection_KeyFieldLengthMismatch_ProducesErrorAndNoOutput()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @( @{ Id = 1; Name = 'Alice' } )");
      shell.InvokeScript("$right = @( @{ Id = 1; Name = 'Bob' } )");

      // LeftKey has 2 fields, RightKey has 1 field - mismatch
      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .LeftKey("Id, Name")
        .RightKey("Id")
        .KeyedJoin(KeyedJoinType.Inner);

      // Act
      var output = shell.InvokeCommandBuilder(command);

      // Assert - validation error should prevent any output
      Assert.IsTrue(shell.HadErrors, "Expected an error for key field length mismatch.");
      Assert.AreEqual(0, output.Count, "Expected no output when validation fails.");

      Assert.IsTrue(shell.Streams.Error.Count > 0, "Expected at least one error record.");
      var errorRecord = shell.Streams.Error[0];
      StringAssert.Contains(errorRecord.FullyQualifiedErrorId, "KeyFieldLengthMismatch");
    }
  }
}
