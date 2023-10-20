using CollectionUtils.JoinCommandHandlers;
using CollectionUtils.Test.CommandBuilders;
using CollectionUtils.Test.Utils;
using System.Collections.Generic;

namespace CollectionUtils.Test
{
  public partial class JoinCollectionPSCmdletTests
  {
    public static IEnumerable<object[]> GetKeyedJoinTestData()
    {
      var joinTypeAndExpectedResults = new[]
      {
        (KeyedJoinType.Disjunct, 1, 0, 1),
        (KeyedJoinType.Inner, 0, 1, 0),
        (KeyedJoinType.Left, 1, 1, 0),
        (KeyedJoinType.Outer, 1, 1, 1),
        (KeyedJoinType.Right, 0, 1, 1),
      };

      var values = new[]
      {
        new (string, object)[] { ("Id", 1), ("Value", "one") },
        new (string, object)[] { ("Id", 2), ("Value", "two") },
        new (string, object)[] { ("Id", 3), ("Value", "three") }
      };

      var left = values[..2];
      var right = values[1..];

      var collections = new[]
      {
        (left.ToPSArrayOfPSCustomObjectsString(), right.ToPSArrayOfPSCustomObjectsString(), "Id"),
        (left.ToPSArrayOfPSHashtableString(), right.ToPSArrayOfPSHashtableString(), "Id"),
        ("@{ 1 = 'one'; 2 = 'two' }", "@{ 2 = 'two'; 3 = 'three' }", "Key")
      };

      foreach (var (l, r, key) in collections)
        foreach (var (joinType, expectedLeftCount, expectedRightCount, expectedInnerCount) in joinTypeAndExpectedResults)
          yield return new object[] { l, r, key, joinType, expectedLeftCount, expectedRightCount, expectedInnerCount };
    }

    [TestMethod]
    [DynamicData(nameof(GetKeyedJoinTestData), DynamicDataSourceType.Method)]
    public void Invoke_UseKeyParameter_CorrectCountOfObjectsReturned(
      string leftCollection,
      string rightCollection,
      string key,
      KeyedJoinType joinType,
      int expectedLeftCount,
      int expectedInnerCount,
      int expectedRightCount)
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript($"$left = {leftCollection}");
      shell.InvokeScript($"$right = {rightCollection}");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .KeyedJoin(joinType)
        .Key(key);

      // Act
      var results =
        shell
        .InvokeCommandBuilder(command)
        .Cast<dynamic>()
        .ToArray();

      var left = results.Where(x => x.Right is null).ToArray();
      var right = results.Where(x => x.Left is null).ToArray();
      var inner = results.Where(x => x.Left is not null && x.Right is not null).ToArray();

      Assert.AreEqual(expectedLeftCount, left.Length);
      Assert.AreEqual(expectedRightCount, right.Length);
      Assert.AreEqual(expectedInnerCount, inner.Length);
    }

    [TestMethod]
    [DynamicData(nameof(GetKeyedJoinTestData), DynamicDataSourceType.Method)]
    public void Invoke_UseKeyParameter_KeyCaseDoesNotMatchPropertyNameCase_CorrectCountOfObjectsReturned(
      string leftCollection,
      string rightCollection,
      string key,
      KeyedJoinType joinType,
      int expectedLeftCount,
      int expectedInnerCount,
      int expectedRightCount)
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript($"$left = {leftCollection}");
      shell.InvokeScript($"$right = {rightCollection}");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .KeyedJoin(joinType)
        .Key(key.ToLower());

      // Act
      var results =
        shell
        .InvokeCommandBuilder(command)
        .Cast<dynamic>()
        .ToArray();

      var left = results.Where(x => x.Right is null).ToArray();
      var right = results.Where(x => x.Left is null).ToArray();
      var inner = results.Where(x => x.Left is not null && x.Right is not null).ToArray();

      Assert.AreEqual(expectedLeftCount, left.Length);
      Assert.AreEqual(expectedRightCount, right.Length);
      Assert.AreEqual(expectedInnerCount, inner.Length);
    }

    [TestMethod]
    [DynamicData(nameof(GetKeyedJoinTestData), DynamicDataSourceType.Method)]
    public void Invoke_UseKeyParameterWithScriptBlock_CorrectCountOfObjectsReturned(
      string leftCollection,
      string rightCollection,
      string key,
      KeyedJoinType joinType,
      int expectedLeftCount,
      int expectedInnerCount,
      int expectedRightCount)
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript($"$left = {leftCollection}");
      shell.InvokeScript($"$right = {rightCollection}");

      var command =
        PSBuilder
        .JoinCollection()
        .Left("$left")
        .Right("$right")
        .KeyedJoin(joinType)
        .Key(PSBuilder.KeyParameter("MyId", $"$_.{key}"));

      // Act
      var temp =
        shell
        .InvokeCommandBuilder(command)
        .ToArray();

      var results =
        temp
        .Cast<dynamic>()
        .ToArray();

      var left = results.Where(x => x.Right is null).ToArray();
      var right = results.Where(x => x.Left is null).ToArray();
      var inner = results.Where(x => x.Left is not null && x.Right is not null).ToArray();

      Assert.AreEqual(expectedLeftCount, left.Length);
      Assert.AreEqual(expectedRightCount, right.Length);
      Assert.AreEqual(expectedInnerCount, inner.Length);
    }

    [TestMethod]
    [DynamicData(nameof(GetKeyedJoinTestData), DynamicDataSourceType.Method)]
    public void Invoke_UseLeftKeyAndRightKeyParameters_CorrectCountOfObjectsReturned(
      string leftCollection,
      string rightCollection,
      string key,
      KeyedJoinType joinType,
      int expectedLeftCount,
      int expectedInnerCount,
      int expectedRightCount)
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript($"$left = {leftCollection}");
      shell.InvokeScript($"$right = {rightCollection}");

      var command =
        PSBuilder
        .JoinCollection()
        .Left("$left")
        .Right("$right")
        .KeyedJoin(joinType)
        .LeftKey(key)
        .RightKey(key);

      // Act
      var results =
        shell
        .InvokeCommandBuilder(command)
        .Cast<dynamic>()
        .ToArray();

      var left = results.Where(x => x.Right is null).ToArray();
      var right = results.Where(x => x.Left is null).ToArray();
      var inner = results.Where(x => x.Left is not null && x.Right is not null).ToArray();

      Assert.AreEqual(expectedLeftCount, left.Length);
      Assert.AreEqual(expectedRightCount, right.Length);
      Assert.AreEqual(expectedInnerCount, inner.Length);
    }

    [TestMethod]
    [DynamicData(nameof(GetKeyedJoinTestData), DynamicDataSourceType.Method)]
    public void Invoke_UseLeftKeyAndRightKeyParametersWithScriptBlock_CorrectCountOfObjectsReturned(
      string leftCollection,
      string rightCollection,
      string key,
      KeyedJoinType joinType,
      int expectedLeftCount,
      int expectedInnerCount,
      int expectedRightCount)
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript($"$left = {leftCollection}");
      shell.InvokeScript($"$right = {rightCollection}");

      var command =
        PSBuilder
        .JoinCollection()
        .Left("$left")
        .Right("$right")
        .KeyedJoin(joinType)
        .LeftKey(PSBuilder.KeyParameter("MyId", $"$_.{key}"))
        .RightKey(PSBuilder.KeyParameter("MyId", $"$_.{key}"));

      // Act
      var results =
        shell
        .InvokeCommandBuilder(command)
        .Cast<dynamic>()
        .ToArray();

      var left = results.Where(x => x.Right is null).ToArray();
      var right = results.Where(x => x.Left is null).ToArray();
      var inner = results.Where(x => x.Left is not null && x.Right is not null).ToArray();

      Assert.AreEqual(expectedLeftCount, left.Length);
      Assert.AreEqual(expectedRightCount, right.Length);
      Assert.AreEqual(expectedInnerCount, inner.Length);
    }

    [TestMethod]
    public void Invoke_LeftKeyCaseDoesNotMatchRightKeyCase_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @( @{ 'Id' = 1 } )");
      shell.InvokeScript("$right = @( @{ 'id' = 1 } )");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .LeftKey("Id")
        .RightKey("id")
        .KeyedJoin(KeyedJoinType.Outer);

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var result =
        output
        .Cast<dynamic>()
        .ToArray();

      Assert.AreEqual(1, result.Length);
    }

    [TestMethod]
    public void Invoke_KeyValueCasesDoNotMatch_DefaultStringComparer_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @( @{ 'Value' = 'one' } )");
      shell.InvokeScript("$right = @( @{ 'Value' = 'One' } )");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .Key("Value")
        .KeyedJoin(KeyedJoinType.Outer);

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var result =
        output
        .Cast<dynamic>()
        .ToArray();

      Assert.AreEqual(1, result.Length);
    }

    [TestMethod]
    [Ignore("Not implemented yet")]
    public void Invoke_LeftKeyIsProperty_RightKeyIsScriptBlock_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @( @{ 'Value' = 'one' } )");
      shell.InvokeScript("$right = @( @{ 'Value' = 'One' } )");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .LeftKey("Value")
        .RightKey(PSBuilder.KeyParameter("Value", $"$_.value"))
        .KeyedJoin(KeyedJoinType.Outer);

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var result =
        output
        .Cast<dynamic>()
        .ToArray();

      Assert.AreEqual(1, result.Length);
    }

    [TestMethod]
    public void Invoke_RightCollectionIsEmpty_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @( @{ 'Value' = 'one' } )");
      shell.InvokeScript("$right = @()");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .Key("Value")
        .KeyedJoin(KeyedJoinType.Outer);

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var result =
        output
        .Cast<dynamic>()
        .ToArray();

      Assert.AreEqual(1, result.Length);
    }

    [TestMethod]
    public void Invoke_RightCollectionIsEmpty_KeyCollisionPreferenceIsGroup_ResultRightIsEmptyArray()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @( @{ 'Value' = 'one' } )");
      shell.InvokeScript("$right = @()");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .Key("Value")
        .KeyedJoin(KeyedJoinType.Outer)
        .KeyCollisionPreference("Group", true);

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var result =
        output
        .Cast<dynamic>()
        .ToArray();

      Assert.AreEqual(1, result.Length);
      Assert.IsNotNull(result[0].Right);
    }

    [TestMethod]
    public void Invoke_LeftCollectionIsEmpty_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @()");
      shell.InvokeScript("$right = @( @{ 'Value' = 'one' } )");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .Key("Value")
        .KeyedJoin(KeyedJoinType.Outer);

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var result =
        output
        .Cast<dynamic>()
        .ToArray();

      Assert.AreEqual(1, result.Length);
    }

    [TestMethod]
    public void Invoke_LeftCollectionIsEmpty_KeyCollisionPreferenceIsGroup_LeftResultIsEmptyArray()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @()");
      shell.InvokeScript("$right = @( @{ 'Value' = 'one' } )");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .Key("Value")
        .KeyedJoin(KeyedJoinType.Outer)
        .KeyCollisionPreference("Group", true);

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var result =
        output
        .Cast<dynamic>()
        .ToArray();

      Assert.AreEqual(1, result.Length);
      Assert.IsNotNull(result[0].Left);
    }

    [TestMethod]
    public void Invoke_ComparerCaseDoesNotMatchLeftKeyCase_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @( @{ 'Value' = 'one' } )");
      shell.InvokeScript("$right = @( @{ 'Value' = 'One' } )");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .Key("Value")
        .Comparer("@{ value = [StringComparer]::OrdinalIgnoreCase }")
        .KeyedJoin(KeyedJoinType.Outer);

      // Act
      var output =
        shell
        .InvokeCommandBuilder(command);

      var result =
        output
        .Cast<dynamic>()
        .ToArray();

      Assert.AreEqual(1, result.Length);
    }
  }
}
