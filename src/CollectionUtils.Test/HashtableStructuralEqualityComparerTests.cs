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
    [TestMethod]
    public void MultipleKeys_SwappedStringValues_HashCodesDoNotCollide()
    {
      // Arrange - this scenario caused collisions with XOR because XOR is commutative
      var left = new Hashtable { { "FirstName", "alice" }, { "LastName", "bob" } };
      var right = new Hashtable { { "FirstName", "bob" }, { "LastName", "alice" } };

      var sut = new HashtableStructuralEqualityComparer("FirstName", "LastName");

      // Act
      var leftHash = sut.GetHashCode(left);
      var rightHash = sut.GetHashCode(right);
      var result = sut.Equals(left, right);

      // Assert
      Assert.IsFalse(result);
      Assert.AreNotEqual(leftHash, rightHash);
    }

    [TestMethod]
    public void MultipleKeys_IdenticalStringValues_HashCodeIsNotZero()
    {
      // Arrange - with XOR, identical values across keys cancel out to zero
      var obj = new Hashtable { { "FirstName", "same" }, { "LastName", "same" } };

      var sut = new HashtableStructuralEqualityComparer("FirstName", "LastName");

      // Act
      var hashCode = sut.GetHashCode(obj);

      // Assert - hash should not degenerate to zero just because the values are equal
      Assert.AreNotEqual(0, hashCode);
    }

    [TestMethod]
    public void MultipleKeys_SwappedIntValues_HashCodesDoNotCollide()
    {
      // Arrange - same as swapped strings but with integers
      var left = new Hashtable { { "A", 1 }, { "B", 2 } };
      var right = new Hashtable { { "A", 2 }, { "B", 1 } };

      var sut = new HashtableStructuralEqualityComparer("A", "B");

      // Act
      var leftHash = sut.GetHashCode(left);
      var rightHash = sut.GetHashCode(right);
      var result = sut.Equals(left, right);

      // Assert
      Assert.IsFalse(result);
      Assert.AreNotEqual(leftHash, rightHash);
    }

    [TestMethod]
    public void MultipleKeys_IdenticalIntValues_HashCodeIsNotZero()
    {
      // Arrange - with XOR, identical int values across keys cancel out to zero
      var obj = new Hashtable { { "A", 42 }, { "B", 42 } };

      var sut = new HashtableStructuralEqualityComparer("A", "B");

      // Act
      var hashCode = sut.GetHashCode(obj);

      // Assert
      Assert.AreNotEqual(0, hashCode);
    }

    [TestMethod]
    public void MultipleKeys_NullValues_HashCodeIsConsistent()
    {
      // Arrange
      var left = new Hashtable { { "A", null }, { "B", null } };
      var right = new Hashtable { { "A", null }, { "B", null } };

      var sut = new HashtableStructuralEqualityComparer("A", "B");

      // Act
      var leftHash = sut.GetHashCode(left);
      var rightHash = sut.GetHashCode(right);
      var result = sut.Equals(left, right);

      // Assert
      Assert.IsTrue(result);
      Assert.AreEqual(leftHash, rightHash);
    }
  }
}
