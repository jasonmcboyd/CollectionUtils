using CollectionUtils.Test.CommandBuilders;
using CollectionUtils.Test.Utils;
using System.Management.Automation;

namespace CollectionUtils.Test
{
  [TestClass]
  public partial class JoinCollectionPSCmdletTests
  {
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
