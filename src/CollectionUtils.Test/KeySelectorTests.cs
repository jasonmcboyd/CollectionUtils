using System.Collections;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class KeySelectorTests
  {
    [TestMethod]
    public void GetKey_SingleIntKeyField_ExpandKeyTrue_CorrectObjectReturned()
    {
      // Arrange
      var keyFields = new KeyField[] { new KeyField("Id") };
      var sut = new KeySelector(keyFields, expandKey: true);

      // Act
      var result = sut.GetKey(new PSObject(new { Id = 1, Value = "one" }));

      // Assert
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetKey_SingleStringKeyField_ExpandKeyTrue_CorrectObjectReturned()
    {
      // Arrange
      var keyFields = new KeyField[] { new KeyField("Value") };
      var sut = new KeySelector(keyFields, expandKey: true);

      // Act
      var result = sut.GetKey(new PSObject(new { Id = 1, Value = "one" }));

      // Assert
      Assert.AreEqual("one", result);
    }

    [TestMethod]
    public void GetKey_SingleKeyField_ExpandKeyTrue_PropertyCasingDoesNotMatch_CorrectObjectReturned()
    {
      // Arrange
      var keyFields = new KeyField[] { new KeyField("value") };
      var sut = new KeySelector(keyFields, expandKey: true);

      // Act
      var result = sut.GetKey(new PSObject(new { Id = 1, Value = "one" }));

      // Assert
      Assert.AreEqual("one", result);
    }

    [TestMethod]
    public void GetKey_MultipleKeyFields_HashtableReturned()
    {
      // Arrange
      var keyFields = new KeyField[] { new KeyField("id"), new KeyField("value") };
      var sut = new KeySelector(keyFields, expandKey: false);

      // Act
      var result = sut.GetKey(new PSObject(new { Id = 1, Value = "one" }));

      // Assert
      Assert.IsInstanceOfType<Hashtable>(result);
      Assert.AreEqual(2, ((Hashtable)result).Count);
      Assert.IsTrue(((Hashtable)result).ContainsKey("id"));
      Assert.IsTrue(((Hashtable)result).ContainsKey("value"));
    }

    [TestMethod]
    public void GetKey_SingleIntKeyField_ExpandKeyFalse_HashtableWithOneEntryReturned()
    {
      // Arrange
      var keyFields = new KeyField[] { new KeyField("Id") };
      var sut = new KeySelector(keyFields, expandKey: false);

      // Act
      var result = sut.GetKey(new PSObject(new { Id = 1, Value = "one" }));

      // Assert
      Assert.IsInstanceOfType<Hashtable>(result);
      var hashtable = (Hashtable)result;
      Assert.AreEqual(1, hashtable.Count);
      Assert.IsTrue(hashtable.ContainsKey("Id"));
      Assert.AreEqual(1, hashtable["Id"]);
    }

    [TestMethod]
    public void GetKey_SingleStringKeyField_ExpandKeyFalse_HashtableWithOneEntryReturned()
    {
      // Arrange
      var keyFields = new KeyField[] { new KeyField("Value") };
      var sut = new KeySelector(keyFields, expandKey: false);

      // Act
      var result = sut.GetKey(new PSObject(new { Id = 1, Value = "one" }));

      // Assert
      Assert.IsInstanceOfType<Hashtable>(result);
      var hashtable = (Hashtable)result;
      Assert.AreEqual(1, hashtable.Count);
      Assert.IsTrue(hashtable.ContainsKey("Value"));
      Assert.AreEqual("one", hashtable["Value"]);
    }

    [TestMethod]
    public void GetKey_SingleKeyField_ExpandKeyFalse_PropertyCasingDoesNotMatch_HashtableWithOneEntryReturned()
    {
      // Arrange
      var keyFields = new KeyField[] { new KeyField("value") };
      var sut = new KeySelector(keyFields, expandKey: false);

      // Act
      var result = sut.GetKey(new PSObject(new { Id = 1, Value = "one" }));

      // Assert
      Assert.IsInstanceOfType<Hashtable>(result);
      var hashtable = (Hashtable)result;
      Assert.AreEqual(1, hashtable.Count);
      Assert.IsTrue(hashtable.ContainsKey("value"));
      Assert.AreEqual("one", hashtable["value"]);
    }
  }
}
