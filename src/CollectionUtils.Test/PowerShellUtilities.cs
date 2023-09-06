using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace CollectionUtils.Test
{
  internal static class PowerShellUtilities
  {
    public static PowerShell CreateShell()
    {
      var testAssemblyPath = Assembly.GetExecutingAssembly().Location;
      var directoryOfTestAssembly = Path.GetDirectoryName(testAssemblyPath)!;

      var shell = PowerShell.Create();

      var absolutePathToModuleDll = Path.GetFullPath(Path.Combine(directoryOfTestAssembly, "CollectionUtils.dll"));
      Console.WriteLine(absolutePathToModuleDll);

      // Import the module
      shell.InvokeScript($"Import-Module '{absolutePathToModuleDll}'");

      return shell;
    }

    public static Collection<PSObject> InvokeScript(this PowerShell shell, string script)
    {
      shell.AddScript(script);
      return shell.Invoke();
    }
  }
}
