using System;

namespace CollectionUtils.Exceptions
{
  public class DuplicateKeyException : Exception
  {
    public DuplicateKeyException(object key, object obj) : base($"Item has already been added:\r\nKey:\r\n  {key}\r\nObject:\r\n  {obj}")
    {
      Key = key;
      Object = obj;
    }

    public object Key { get; }
    public object Object { get; }
  }
}
