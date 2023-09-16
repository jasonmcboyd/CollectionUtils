using CollectionUtils.Test.CommandBuilders;
using CollectionUtils.Test.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class JoinCollectionPSCmdletTests
  {
    private IEnumerable<(int, string)> GetData()
    {
      yield return (1, "One");
      yield return (2, "Two");
      yield return (3, "Three");
      yield return (4, "Three");
      yield return (5, "Three");
      yield return (6, "Three");
    }

    private IEnumerable<(int, string)> GetRightData()
    {
      yield return (1, "One");
      yield return (2, "Two-A");
      yield return (2, "Two-B");
      yield return (3, "three");
    }

    private Hashtable TupleToHashTable((int, string) tuple)
    {
      var hashTable = new Hashtable
      {
        { "Id", tuple.Item1 },
        { "Value", tuple.Item2 }
      };

      return hashTable;
    }

    private Dictionary<int, object> TupleToGenericDictionary((int, string) tuple)
    {
      var dictionary = new Dictionary<int, object>
      {
        { tuple.Item1, tuple.Item2 }
      };

      return dictionary;
    }

    private IEnumerable<object> GetObjects(int start, int count)
    {
      foreach (var num in Enumerable.Range(start, count))
        yield return new { Num = num, NumString = num.ToString() };
    }

    [TestMethod]
    [DataRow(KeyedJoinType.Outer, 1, 1, 1)]
    [DataRow(KeyedJoinType.Left, 1, 0, 1)]
    [DataRow(KeyedJoinType.Right, 0, 1, 1)]
    [DataRow(KeyedJoinType.Inner, 0, 0, 1)]
    public void Invoke_UseKeyParameter_InputIsPSCustomObjects_CorrectCountOfObjectsReturned(KeyedJoinType joinType, int expectedLeftCount, int expectedRightCount, int expectedInnerCount)
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @([pscustomobject]@{ Id = 1; Value = 'one' }, [pscustomobject]@{ Id = 2; Value = 'two' })");

      shell.InvokeScript("$right = @([pscustomobject]@{ Id = 2; Value = 'two' }, [pscustomobject]@{ Id = 3; Value = 'three' })");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .KeyedJoin(joinType)
        .Key("Id");

      // Act
      var results =
        shell
        .InvokeCommandBuilder(command)
        .Cast<dynamic>()
        .ToArray();

      var left = results.Where(x => x.Right == null).ToArray();
      var right = results.Where(x => x.Left == null).ToArray();
      var inner = results.Where(x => x.Left != null && x.Right != null).ToArray();

      Assert.AreEqual(expectedLeftCount, left.Length);
      Assert.AreEqual(expectedRightCount, right.Length);
      Assert.AreEqual(expectedInnerCount, inner.Length);
    }

    [TestMethod]
    [DataRow(KeyedJoinType.Outer, 1, 1, 1)]
    [DataRow(KeyedJoinType.Left, 1, 0, 1)]
    [DataRow(KeyedJoinType.Right, 0, 1, 1)]
    [DataRow(KeyedJoinType.Inner, 0, 0, 1)]
    public void Invoke_UseKeyParameter_CorrectCountOfObjectsReturned(KeyedJoinType joinType, int expectedLeftCount, int expectedRightCount, int expectedInnerCount)
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      shell.InvokeScript("$right = @(@{ Id = 2; Value = 'two' }, @{ Id = 3; Value = 'three' })");

      var command =
        new JoinCollectionCommandBuilder()
        .Left("$left")
        .Right("$right")
        .KeyedJoin(joinType)
        .Key("Id");

      // Act
      var results =
        shell
        .InvokeCommandBuilder(command)
        .Cast<dynamic>()
        .ToArray();

      var left = results.Where(x => x.Right == null).ToArray();
      var right = results.Where(x => x.Left == null).ToArray();
      var inner = results.Where(x => x.Left != null && x.Right != null).ToArray();

      Assert.AreEqual(expectedLeftCount, left.Length);
      Assert.AreEqual(expectedRightCount, right.Length);
      Assert.AreEqual(expectedInnerCount, inner.Length);
    }

    [TestMethod]
    [DataRow(KeyedJoinType.Outer, 1, 1, 1)]
    [DataRow(KeyedJoinType.Left, 1, 0, 1)]
    [DataRow(KeyedJoinType.Right, 0, 1, 1)]
    [DataRow(KeyedJoinType.Inner, 0, 0, 1)]
    public void Invoke_UseKeyParameterWithScriptBlock_CorrectCountOfObjectsReturned(KeyedJoinType joinType, int expectedLeftCount, int expectedRightCount, int expectedInnerCount)
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      shell.InvokeScript("$right = @(@{ Id = 2; Value = 'two' }, @{ Id = 3; Value = 'three' })");

      var command =
        PSBuilder
        .JoinCollection()
        .Left("$left")
        .Right("$right")
        .KeyedJoin(joinType)
        .Key(PSBuilder.KeyParameter("MyId", "$_.Id"));

      // Act
      var temp =
        shell
        .InvokeCommandBuilder(command)
        .ToArray();
      
      var results =
        temp
        .Cast<dynamic>()
        .ToArray();

      var left = results.Where(x => x.Right == null).ToArray();
      var right = results.Where(x => x.Left == null).ToArray();
      var inner = results.Where(x => x.Left != null && x.Right != null).ToArray();

      Assert.AreEqual(expectedLeftCount, left.Length);
      Assert.AreEqual(expectedRightCount, right.Length);
      Assert.AreEqual(expectedInnerCount, inner.Length);
    }

    [TestMethod]
    [DataRow(KeyedJoinType.Outer, 1, 1, 1)]
    [DataRow(KeyedJoinType.Left, 1, 0, 1)]
    [DataRow(KeyedJoinType.Right, 0, 1, 1)]
    [DataRow(KeyedJoinType.Inner, 0, 0, 1)]
    public void Invoke_UseLeftKeyAndRightKeyParameters_CorrectCountOfObjectsReturned(KeyedJoinType joinType, int expectedLeftCount, int expectedRightCount, int expectedInnerCount)
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = @(@{ Id = 1; Value = 'one' }, @{ Id = 2; Value = 'two' })");

      shell.InvokeScript("$right = @(@{ Id = 2; Value = 'two' }, @{ Id = 3; Value = 'three' })");

      var command =
        PSBuilder
        .JoinCollection()
        .Left("$left")
        .Right("$right")
        .KeyedJoin(joinType)
        .LeftKey("Id")
        .RightKey("Id");

      // Act
      var results =
        shell
        .InvokeCommandBuilder(command)
        .Cast<dynamic>()
        .ToArray();

      var left = results.Where(x => x.Right == null).ToArray();
      var right = results.Where(x => x.Left == null).ToArray();
      var inner = results.Where(x => x.Left != null && x.Right != null).ToArray();

      Assert.AreEqual(expectedLeftCount, left.Length);
      Assert.AreEqual(expectedRightCount, right.Length);
      Assert.AreEqual(expectedInnerCount, inner.Length);
    }

    [TestMethod]
    public void Invoke_CrossJoin_CorrectResultsReturned()
    {
      // Arrange
      using var shell = PowerShellUtilities.CreateShell();

      shell.InvokeScript("$left = 0..3");

      shell.InvokeScript("$right = 4..7");

      var command =
        PSBuilder
        .JoinCollection()
        .Left("$left")
        .Right("$right")
        .CrossJoin();

      // Act
      var results =
        shell
        .InvokeCommandBuilder(command)
        .Cast<PSObject>()
        .ToArray();

      Assert.AreEqual(16, results.Length);
    }
  }
}
