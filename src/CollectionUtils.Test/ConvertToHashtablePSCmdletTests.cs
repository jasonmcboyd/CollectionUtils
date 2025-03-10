﻿using CollectionUtils.PSCmdlets;
using CollectionUtils.Test.CommandBuilders;
using CollectionUtils.Test.Utils;
using System.Collections;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class ConvertToHashtablePSCmdletTests
  {
    [TestMethod]
    public void InvokeWithoutPipeline_SingleKeyField_KeyValueIsInt_ResultKeyIsInt()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @( @{ Id = 1; Value = 'one' } )");

      var script =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Id")
        .ToString();

      // Act
      var output =
        shell
        .InvokeScript(script);

      var results =
        output
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(1, results.Count);
      Assert.IsTrue(results.Cast<DictionaryEntry>().All(x => x.Key is int));
    }

    [TestMethod]
    public void InvokeWithoutPipeline_SingleKeyField_KeyValueIsString_ResultKeyIsString()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @( @{ Id = 1; Value = 'one' } )");

      var script = "ConvertTo-Hashtable -InputObject $objs -Key Value";

      // Act
      var output =
        shell
        .InvokeScript(script);

      var results =
        output
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(1, results.Count);
      Assert.IsTrue(results.Cast<DictionaryEntry>().All(x => x.Key is string));
    }

    [TestMethod]
    public void InvokeWitoutPipeline_MultipleKeyFields_ResultKeyIsHashtable()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      var script =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Id, Value")
        .ToString();

      // Act
      var output =
        shell
        .InvokeScript(script);

      var results =
        output
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(2, results.Count);
      Assert.IsTrue(results.Cast<DictionaryEntry>().All(x => x.Key is Hashtable));
    }

    [TestMethod]
    public void InvokeWithoutPipeline_KeyFieldIsHashtable_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("@{ TestId = { $_.Id } }")
        .KeyCollisionPreference(ConvertToHashtableKeyCollisionPreference.Group.ToString());

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var results =
        output
        .Cast<PSObject>()
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(2, results.Count);
      //Assert.AreEqual(new Hashtable { { "Id", 1 }, { "Value", "one" } }, results[1], new HashtableStructuralEqualityComparer(new[] {  } );
      //Assert.AreEqual("two", results[2]);
    }

    [TestMethod]
    public void InvokeWithoutPipeline_KeyParameterIsHashtableWithMultipleValues_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("@{ TestId = { $_.Id }; TestValue = { $_.Value } }")
        .KeyCollisionPreference(ConvertToHashtableKeyCollisionPreference.Group.ToString());

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var results =
        output
        .Cast<PSObject>()
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(2, results.Count);
      Assert.IsTrue(results.Cast<DictionaryEntry>().All(x => x.Key is Hashtable));
    }

    [TestMethod]
    public void InvokeWithoutPipeline_MixedCaseKeys_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Value = 'one' }, @{ Value = 'ONE' })");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Value")
        .KeyCollisionPreference(ConvertToHashtableKeyCollisionPreference.Group.ToString());

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var results =
        output
        .Cast<PSObject>()
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(1, results.Count);
    }

    [TestMethod]
    public void InvokeWithoutPipeline_MixedCaseKeys_DefaultStringComparerIsCaseSensitive_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Value = 'one' }, @{ Value = 'ONE' })");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Value")
        .DefaultStringComparer("([System.StringComparer]::Ordinal)");

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var results =
        output
        .Cast<PSObject>()
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(2, results.Count);
    }

    [TestMethod]
    public void InvokeWithoutPipeline_MixedCaseKeys_ComparerIsCaseSensitive_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Value = 'one' }, @{ Value = 'ONE' })");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Value")
        .Comparer("@{ Value = [System.StringComparer]::Ordinal }");

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var results =
        output
        .Cast<PSObject>()
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(2, results.Count);
    }

    [TestMethod]
    public void InvokeWithoutPipeline_MixedCaseKeyNames_ComparerPassed_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = @(@{ Value = 'one' }, @{ value = 'ONE' })");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key("Value")
        .Comparer("@{ Value = [System.StringComparer]::Ordinal }");

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var results =
        output
        .Cast<PSObject>()
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(2, results.Count);
    }

    [TestMethod]
    public void InvokeWithoutPipeline_KeyCollisionPreferenceIsGroup_ValuesAreArrays()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$objs = 0..9");

      var command =
        PSBuilder
        .ConvertToHashTable()
        .InputObject("$objs")
        .Key(PSBuilder.KeyParameter("Mod", "$_ % 3"))
        .KeyCollisionPreference(ConvertToHashtableKeyCollisionPreference.Group.ToString());

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var temp =
        output.ToArray();

      var results =
        temp
        .Cast<PSObject>()
        .Select(x => x.BaseObject)
        .Cast<Hashtable>()
        .Single();

      Assert.AreEqual(3, results.Count);
      PSObjectCollectionAssert.AreEqual(results.Get<PSObject[]>(0), new[] { 0, 3, 6, 9 });
      PSObjectCollectionAssert.AreEqual(results.Get<PSObject[]>(1), new[] { 1, 4, 7 });
      PSObjectCollectionAssert.AreEqual(results.Get<PSObject[]>(2), new[] { 2, 5, 8 });
    }
  }
}
