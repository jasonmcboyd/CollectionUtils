using System.Collections;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public class KeySelectorTests
  {
    [TestMethod]
    public void GetKey_SingleIntKeyField_CorrectObjectReturned()
    {
      // Arrange
      var keyFields = new KeyField[] { new KeyField("Id") };
      var sut = new KeySelector(keyFields);

      // Act
      var result = sut.GetKey(new PSObject(new { Id = 1, Value = "one" }));

      // Assert
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetKey_SingleStringKeyField_CorrectObjectReturned()
    {
      // Arrange
      var keyFields = new KeyField[] { new KeyField("Value") };
      var sut = new KeySelector(keyFields);

      // Act
      var result = sut.GetKey(new PSObject(new { Id = 1, Value = "one" }));

      // Assert
      Assert.AreEqual("one", result);
    }

    [TestMethod]
    public void GetKey_SingleKeyField_PropertyCasingDoesNotMatch_CorrectObjectReturned()
    {
      // Arrange
      var keyFields = new KeyField[] { new KeyField("value") };
      var sut = new KeySelector(keyFields);

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
      var sut = new KeySelector(keyFields);

      // Act
      var result = sut.GetKey(new PSObject(new { Id = 1, Value = "one" }));

      // Assert
      Assert.IsInstanceOfType<Hashtable>(result);
      Assert.AreEqual(2, ((Hashtable)result).Count);
      Assert.IsTrue(((Hashtable)result).ContainsKey("id"));
      Assert.IsTrue(((Hashtable)result).ContainsKey("value"));
    }
  }
}
