using System;

namespace CollectionUtils.Exceptions
{
  public class ScriptBlockEmptyResultException : Exception
  {
    public ScriptBlockEmptyResultException(object targetObject)
      : base("Key expression script block returned no results. Ensure the script block produces a value (e.g., { $_.PropertyName } instead of { }).")
    {
      TargetObject = targetObject;
    }

    public object TargetObject { get; }
  }
}
