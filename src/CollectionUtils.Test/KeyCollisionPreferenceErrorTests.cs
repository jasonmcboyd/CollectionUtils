using CollectionUtils.PSCmdlets;
using CollectionUtils.Test.CommandBuilders;
using CollectionUtils.Test.Utils;
using System.Collections;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class KeyCollisionPreferenceErrorTests
  {
    [TestMethod]
    public void ConvertToHashtable_KeyCollisionPreferenceIsError_DuplicateKeys_ProducesError()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 1; Value = 'duplicate' })");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Id")
        .KeyCollisionPreference(ConvertToHashtableKeyCollisionPreference.Error.ToString());

      // Act
      var output = shell.InvokeCommandBuilder(command);

      // Assert - should have errors and no output (terminating error stops processing)
      Assert.IsTrue(shell.HadErrors, "Expected terminating error for duplicate key collision.");
      Assert.IsTrue(shell.Streams.Error.Count > 0, "Expected at least one error record.");
      Assert.AreEqual(0, output.Count, "Expected no output when key collision terminates processing.");

      var errorRecord = shell.Streams.Error[0];
      StringAssert.StartsWith(errorRecord.FullyQualifiedErrorId, "KeyCollision");
    }

    [TestMethod]
    public void ConvertToHashtable_KeyCollisionPreferenceIsError_NoDuplicateKeys_ReturnsHashtable()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Id")
        .KeyCollisionPreference(ConvertToHashtableKeyCollisionPreference.Error.ToString());

      // Act
      var output = shell.InvokeCommandBuilder(command);

      var results =
        output
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      // Assert
      Assert.IsFalse(shell.HadErrors, "Expected no errors when there are no duplicate keys.");
      Assert.AreEqual(2, results.Count);
    }

    [TestMethod]
    public void ConvertToHashtable_DefaultKeyCollisionPreference_DuplicateKeys_ProducesError()
    {
      // Arrange - default KeyCollisionPreference is Error
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 1; Value = 'duplicate' })");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Id");

      // Act
      var output = shell.InvokeCommandBuilder(command);

      // Assert
      Assert.IsTrue(shell.HadErrors, "Expected terminating error for duplicate key collision with default preference.");
      Assert.AreEqual(0, output.Count, "Expected no output when key collision terminates processing.");
    }

    [TestMethod]
    public void ConvertToHashtable_KeyCollisionPreferenceIsIgnore_DuplicateKeys_ReturnsFirstValue()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 1; Value = 'duplicate' })");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Id")
        .KeyCollisionPreference(ConvertToHashtableKeyCollisionPreference.Ignore.ToString());

      // Act
      var output = shell.InvokeCommandBuilder(command);

      var results =
        output
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      // Assert - should have only 1 entry (the first one wins), no errors
      Assert.IsFalse(shell.HadErrors, "Expected no errors when KeyCollisionPreference is Ignore.");
      Assert.AreEqual(1, results.Count);
    }

    [TestMethod]
    public void JoinCollection_KeyCollisionPreferenceIsError_DuplicateKeysInRight_ProducesError()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @( @{ Id = 1; Value = 'left' } )");
      shell.InvokeScript("$right = @( @{ Id = 1; Value = 'right1' }, @{ Id = 1; Value = 'right2' } )");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .Key("Id")
        .KeyedJoin(JoinCommandHandlers.KeyedJoinType.Inner)
        .KeyCollisionPreference("Error");

      // Act
      var output = shell.InvokeCommandBuilder(command);

      // Assert
      Assert.IsTrue(shell.HadErrors, "Expected terminating error for duplicate key collision in Join-Collection.");
      Assert.IsTrue(shell.Streams.Error.Count > 0, "Expected at least one error record.");
      Assert.AreEqual(0, output.Count, "Expected no output when key collision terminates processing.");
    }

    [TestMethod]
    public void JoinCollection_KeyCollisionPreferenceIsError_NoDuplicateKeys_ReturnsResults()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @( @{ Id = 1; Value = 'left' } )");
      shell.InvokeScript("$right = @( @{ Id = 1; Value = 'right' } )");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .Key("Id")
        .KeyedJoin(JoinCommandHandlers.KeyedJoinType.Inner)
        .KeyCollisionPreference("Error");

      // Act
      var output = shell.InvokeCommandBuilder(command);

      // Assert
      Assert.IsFalse(shell.HadErrors, "Expected no errors when there are no duplicate keys.");
      Assert.AreEqual(1, output.Count);
    }
  }
}
