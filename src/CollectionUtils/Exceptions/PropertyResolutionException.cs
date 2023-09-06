using System;

namespace CollectionUtils.Exceptions
{
  public class PropertyResolutionException : Exception
  {
    public PropertyResolutionException(
      object targetObject,
      string propertyName)
      : base($"Unable able to resolve property: {propertyName}")
    {
      PropertyName = propertyName;
      TargetObject = targetObject;
    }

    public string PropertyName { get; }
    public object TargetObject { get; }
  }
}
