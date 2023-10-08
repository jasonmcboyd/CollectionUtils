using System;
using System.Collections;

namespace CollectionUtils.Test
{
  [TestClass]
  public class HashtableStructuralEqualityComparerTests
  {
    private Hashtable GetTestData(bool keysAreUpperCase = false)
    {
      var result = new Hashtable(StringComparer.OrdinalIgnoreCase)
      {
        { keysAreUpperCase ? "ID" : "id", 1 },
        { keysAreUpperCase ? "VALUE" : "value", "one" }
      };

      return result;
    }

    [TestMethod]
    public void SingleKey_KeyIsInt_CorrectResults()
    {
      // Arrange
      var left = GetTestData();
      var right = GetTestData();

      var sut = new HashtableStructuralEqualityComparer("id");

      // Act
      var result = sut.Equals(left, right);
      var leftGuid = sut.GetHashCode(left);
      var rightGuid = sut.GetHashCode(right);

      // Assert
      Assert.IsTrue(result);
      Assert.AreEqual(leftGuid, rightGuid);
    }

    [TestMethod]
    public void SingleKey_KeyIsString_CorrectResults()
    {
      // Arrange
      var left = GetTestData();
      var right = GetTestData();

      var sut = new HashtableStructuralEqualityComparer("id");

      // Act
      var result = sut.Equals(left, right);
      var leftGuid = sut.GetHashCode(left);
      var rightGuid = sut.GetHashCode(right);

      // Assert
      Assert.IsTrue(result);
      Assert.AreEqual(leftGuid, rightGuid);
    }

    [TestMethod]
    public void SingleKey_KeyCasesDiffer_CorrectResults()
    {
      // Arrange
      var left = GetTestData();
      var right = GetTestData(true);

      var sut = new HashtableStructuralEqualityComparer("id");

      // Act
      var result = sut.Equals(left, right);
      var leftGuid = sut.GetHashCode(left);
      var rightGuid = sut.GetHashCode(right);

      // Assert
      Assert.IsTrue(result);
      Assert.AreEqual(leftGuid, rightGuid);
    }

    [TestMethod]
    public void MultipleKeys_KeyCasesDiffer_CorrectResults()
    {
      // Arrange
      var left = GetTestData();
      var right = GetTestData(true);

      var sut = new HashtableStructuralEqualityComparer("id", "value");

      // Act
      var result = sut.Equals(left, right);
      var leftGuid = sut.GetHashCode(left);
      var rightGuid = sut.GetHashCode(right);

      // Assert
      Assert.IsTrue(result);
      Assert.AreEqual(leftGuid, rightGuid);
    }

    [TestMethod]
    public void MultipleKeys_CorrectResults()
    {
      // Arrange
      var left = GetTestData();
      var right = GetTestData();

      var sut = new HashtableStructuralEqualityComparer("id", "value");

      // Act
      var result = sut.Equals(left, right);
      var leftGuid = sut.GetHashCode(left);
      var rightGuid = sut.GetHashCode(right);

      // Assert
      Assert.IsTrue(result);
      Assert.AreEqual(leftGuid, rightGuid);
    }

    [TestMethod]
    public void SingleKey_KeyIsString_KeyValueCasesDiffer_CorrectResults()
    {
      // Arrange
      var left = GetTestData();
      var right = GetTestData();

      right["value"] = "ONE";

      var sut = new HashtableStructuralEqualityComparer("value");

      // Act
      var result = sut.Equals(left, right);
      var leftGuid = sut.GetHashCode(left);
      var rightGuid = sut.GetHashCode(right);

      // Assert
      Assert.IsTrue(result);
      Assert.AreEqual(leftGuid, rightGuid);
    }

    [TestMethod]
    public void SingleKey_KeyIsString_KeyValueCasesDiffer_DefaultStringComparerIsCaseSensitive_CorrectResults()
    {
      // Arrange
      var left = GetTestData();
      var right = GetTestData();

      right["value"] = "ONE";

      var sut = new HashtableStructuralEqualityComparer(new[] { new KeyComparer("value") }, StringComparer.Ordinal );

      // Act
      var result = sut.Equals(left, right);
      var leftGuid = sut.GetHashCode(left);
      var rightGuid = sut.GetHashCode(right);

      // Assert
      Assert.IsFalse(result);
      Assert.AreNotEqual(leftGuid, rightGuid);
    }

    [TestMethod]
    public void SingleKey_KeyIsString_KeyValueCasesDiffer_KeyComparerIsCaseSensitive_CorrectResults()
    {
      // Arrange
      var left = GetTestData();
      var right = GetTestData();

      right["value"] = "ONE";

      var sut = new HashtableStructuralEqualityComparer(new KeyComparer("value", StringComparer.Ordinal));

      // Act
      var result = sut.Equals(left, right);
      var leftGuid = sut.GetHashCode(left);
      var rightGuid = sut.GetHashCode(right);

      // Assert
      Assert.IsFalse(result);
      Assert.AreNotEqual(leftGuid, rightGuid);
    }

    [TestMethod]
    public void MultipleKeys_KeysAreStrings_KeyValueCasesDiffer_SingleKeyComparerIsCaseSensitive_CorrectResults()
    {
      // Arrange
      var left = new Hashtable { { "FirstName", "jason" }, { "LastName", "boyd" } };
      var right = new Hashtable { { "FirstName", "JASON" }, { "LastName", "BOYD" } };

      var sut = new HashtableStructuralEqualityComparer(new KeyComparer("FirstName"), new KeyComparer("LastName", StringComparer.Ordinal));

      // Act
      var result = sut.Equals(left, right);
      var leftGuid = sut.GetHashCode(left);
      var rightGuid = sut.GetHashCode(right);

      // Assert
      Assert.IsFalse(result);
      Assert.AreNotEqual(leftGuid, rightGuid);
    }
  }
}
